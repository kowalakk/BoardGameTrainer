﻿using Cairo;
using Game.IGame;
using Gdk;
using Gtk;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using Action = System.Action;

namespace BoardGameTrainer
{
    enum WindowState
    {
        Idle, ProcessMovement, ComputeHints
    }
    internal class MainWindow : Gtk.Window
    {
        public IGameManager? GameManager { get; set; } = null;

        private readonly DrawingArea boardImage = new();
        private readonly ConfigWindow configWindow;
        private readonly Thread handleEventThread;
        private readonly BlockingCollection<Action> eventsQueue = new(new ConcurrentQueue<Action>());

        private CancellationTokenSource tokenSource = new();
        private WindowState windowState = WindowState.Idle;
        private double x;
        private double y;
        public MainWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            DefaultSize = new Size(700, 500);

            handleEventThread = new Thread(new ThreadStart(ThreadHandleEvents));
            handleEventThread.Start();

            configWindow = new(this);
            DeleteEvent += (sender, args) => configWindow.Dispose();
            application.AddWindow(configWindow);

            loadDllFiles();

            Button newGameButton = new("New Game");
            newGameButton.Clicked += (s, e) =>
            {
                configWindow.Show();
            };
            newGameButton.Show();

            Button restartButton = new("Restart");
            restartButton.Clicked += (s, e) =>
            {
                if (GameManager is not null)
                {
                    ResetToken();
                    GameManager.Reset();
                    StartGame();
                }
            };
            restartButton.Show();

            Button addGameButton = new("Add Game");
            addGameButton.Clicked += (s, e) =>
            {
                FileChooserDialog dialog = new("", this, FileChooserAction.Open, "Open",
                    ResponseType.Accept, "Cancel", ResponseType.Cancel);
                FileFilter filter = new()
                {
                    Name = "DLL Files",
                };
                filter.AddPattern("*.dll");
                dialog.AddFilter(filter);

                if (dialog.Run() == (int)ResponseType.Accept)
                {
                    Assembly assembly = Assembly.LoadFile(dialog.Filename);
                    bool isCorrectDll = true;
                    try
                    {
                        IGameManagerFactory factory = LoadGameFactory(assembly);
                        configWindow.UpdateGameFactoryDict(factory);
                    }
                    catch
                    {
                        isCorrectDll= false;
                        MessageDialog error = new(dialog, DialogFlags.DestroyWithParent, MessageType.Error, 
                            ButtonsType.Ok, "Failed to add a new game");
                        error.Run();
                        error.Dispose();
                    }
                    finally
                    {
                        if(isCorrectDll)
                        {
                            copyDllToAppdata(assembly.GetName(), dialog.Filename);
                        }
                        dialog.Dispose();
                    }
                }
            };
            addGameButton.Show();

            HBox panelHbox = new();
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);
            panelHbox.PackStart(addGameButton, false, false, 0);
            panelHbox.Show();

            boardImage.Drawn += (sender, args) =>
            {
                Context context = args.Cr;

                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                context.Translate(xOffset, yOffset);
                context.Scale(minDimention, minDimention);

                GameManager?.DrawBoard(context);
            };
            boardImage.AddEvents((int)EventMask.ButtonPressMask);
            boardImage.ButtonPressEvent += BoardImageClickHandler;
            boardImage.Show();

            HBox contentHBox = new();
            contentHBox.PackStart(boardImage, true, true, 0);
            contentHBox.Show();

            Label mainTitle = new("New Game");
            mainTitle.Show();

            VBox titleAndContentVBox = new();
            titleAndContentVBox.PackStart(mainTitle, false, false, 0);
            titleAndContentVBox.PackStart(contentHBox, true, true, 0);
            titleAndContentVBox.Show();

            VBox mainVBox = new();
            mainVBox.PackStart(panelHbox, false, false, 0);
            mainVBox.PackStart(titleAndContentVBox, true, true, 0);
            mainVBox.Show();
            Add(mainVBox);
        }

        private void copyDllToAppdata(AssemblyName assemblyName, string filename)
        {
            string boardGameTrainerPath = getAppdataPath();
            Directory.CreateDirectory(boardGameTrainerPath);
            StringBuilder pathBuilder = new StringBuilder(boardGameTrainerPath);
            pathBuilder.Append('\\');
            pathBuilder.Append(assemblyName);
            string destinationDllPath = pathBuilder.ToString();
            File.Copy(filename, destinationDllPath, true);
        }

        private string getAppdataPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            StringBuilder pathBuilder = new StringBuilder();
            pathBuilder.Append(appDataPath);
            pathBuilder.Append("\\BoardGameTrainer");
            return pathBuilder.ToString();
        }

        private void loadDllFiles()
        {
            string appdataPath = getAppdataPath();
            if(Directory.Exists(appdataPath))
            {
                foreach (var dll in Directory.GetFiles(appdataPath))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(dll);
                        IGameManagerFactory factory = LoadGameFactory(assembly);
                        configWindow.UpdateGameFactoryDict(factory);
                    }
                    catch
                    {
                        Console.WriteLine("incorrect file in appData/BoardGameTrainer");
                    }
                }
            }
        }

        private void BoardImageClickHandler(object sender, ButtonPressEventArgs args)
        {
            if (windowState != WindowState.ProcessMovement && GameManager is not null)
            {
                windowState = WindowState.ProcessMovement;
                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                x = (args.Event.X - xOffset) / minDimention;
                y = (args.Event.Y - yOffset) / minDimention;

                PerformMovement();
            }
        }

        internal void StartGame()
        {
            windowState = WindowState.Idle;
            if (GameManager!.HumanPlayers[Player.One])
            {
                windowState = WindowState.ComputeHints;
                eventsQueue.Add(ComputeHints);
            }
            else
            {
                windowState = WindowState.ProcessMovement;
                eventsQueue.Add(PerformAiMovement);
            }
        }

        private void PerformMovement()
        {
            (GameResult gameResult, bool isActionPerformed) = GameManager!.HandleMovement(x, y);
            Application.Invoke(delegate { boardImage.QueueDraw(); });
            if (isActionPerformed && gameResult == GameResult.InProgress)
            {
                CancellationToken token = ResetToken();
                Player opponent = GameManager!.CurrentPlayer();
                if (GameManager.HumanPlayers[opponent])
                {
                    windowState = WindowState.ComputeHints;
                    eventsQueue.Add(ComputeHints);
                }
                else
                {
                    eventsQueue.Add(PerformAiMovement);
                }
            }
            else
            {
                windowState = WindowState.Idle;
            }
        }

        private void PerformAiMovement()
        {
            GameResult gameResult = GameManager!.HandleAiMovement();
            Application.Invoke(delegate { boardImage.QueueDraw(); });
            if (gameResult == GameResult.InProgress)
            {
                Player opponent = GameManager!.CurrentPlayer();
                if (GameManager.HumanPlayers[opponent])
                {
                    windowState = WindowState.ComputeHints;
                    eventsQueue.Add(ComputeHints);
                }
                else
                {
                    eventsQueue.Add(PerformAiMovement);
                }
            }
            else
            {
                windowState = WindowState.Idle;
            }
        }

        private void ComputeHints()
        {
            CancellationToken token = ResetToken();
            GameManager!.ComputeHints(token);
            Application.Invoke(delegate
            {
                boardImage.QueueDraw();
                windowState = WindowState.Idle;
            });
        }

        private void ThreadHandleEvents()
        {
            while (true)
            {
                Action job = eventsQueue.Take();
                job();
            }
        }

        private CancellationToken ResetToken()
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }

        private IGameManagerFactory LoadGameFactory(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterface("IGameManagerFactory") != null)
                {
                    object? obj = Activator.CreateInstance(type);
                    IGameManagerFactory gameManagerFactory = (IGameManagerFactory)obj!;
                    return gameManagerFactory;
                }
            }
            throw new Exception("There are no valid plugin(s) in the DLL.");
        }
    }
}
