using Cairo;
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
        private readonly DrawingArea boardImage = new();
        private readonly ConfigWindow configWindow;
        private readonly Thread handleEventThread;
        private readonly BlockingCollection<Action> eventsQueue = new(new ConcurrentQueue<Action>());
        private readonly Label gameTitle = new();
        private readonly SpinButtonBox hintsBox = new(0, 100, 1, 10, "hints shown");

        private IGameManager? gameManager = null;
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

            LoadDllFiles();

            HBox navHBox = CreateNavBox();
            navHBox.Show();

            VBox contentVBox = CreateContentBox();
            contentVBox.Show();

            VBox mainVBox = new();
            mainVBox.PackStart(navHBox, false, false, 0);
            mainVBox.PackStart(contentVBox, true, true, 0);
            mainVBox.Show();
            Add(mainVBox);
        }

        public void CreateGameManager(
            IGameManagerFactory managerFactory,
            IAiFactory aiFactory,
            IStopCondition stopCondition,
            Dictionary<Player, bool> humanPlayers,
            Dictionary<Player, bool> showHints)
        {

            gameManager = managerFactory.Create(aiFactory, stopCondition, humanPlayers, showHints);
            gameManager.NumberOfHints = (int)hintsBox.Value;
            gameTitle.Text = managerFactory.Name;
            StartGame();
        }

        private static void CopyDllToAppdata(AssemblyName assemblyName, string filename)
        {
            string boardGameTrainerPath = GetAppdataPath();
            Directory.CreateDirectory(boardGameTrainerPath);
            StringBuilder pathBuilder = new StringBuilder(boardGameTrainerPath);
            pathBuilder.Append('\\');
            pathBuilder.Append(assemblyName);
            string destinationDllPath = pathBuilder.ToString();
            File.Copy(filename, destinationDllPath, true);
        }

        private static string GetAppdataPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            StringBuilder pathBuilder = new StringBuilder();
            pathBuilder.Append(appDataPath);
            pathBuilder.Append("\\BoardGameTrainer");
            return pathBuilder.ToString();
        }

        private static IGameManagerFactory LoadGameFactory(Assembly assembly)
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

        private void StartGame()
        {
            windowState = WindowState.Idle;
            if (gameManager!.HumanPlayers[Player.One])
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

        private HBox CreateNavBox()
        {
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
                        isCorrectDll = false;
                        MessageDialog error = new(dialog, DialogFlags.DestroyWithParent, MessageType.Error,
                            ButtonsType.Ok, "Failed to add a new game");
                        error.Run();
                        error.Dispose();
                    }
                    if (isCorrectDll)
                    {
                        CopyDllToAppdata(assembly.GetName(), dialog.Filename);
                    }
                }
                dialog.Dispose();
            };
            addGameButton.Show();

            Button newGameButton = new("New Game");
            newGameButton.Clicked += (s, e) =>
            {
                configWindow.Show();
            };
            newGameButton.Show();

            Button restartButton = new("Restart");
            restartButton.Clicked += (s, e) =>
            {
                if (gameManager is not null)
                {
                    ResetToken();
                    gameManager.Reset();
                    StartGame();
                }
            };
            restartButton.Show();
            
            hintsBox.Changed += (sender, args) =>
            {
                if (gameManager is not null)
                {
                    gameManager.NumberOfHints = (int)hintsBox.Value;
                    Application.Invoke(delegate { boardImage.QueueDraw(); });
                }
            };
            hintsBox.Show();

            HBox navHBox = new();
            navHBox.PackStart(addGameButton, false, false, 0);
            navHBox.PackStart(newGameButton, false, false, 0);
            navHBox.PackStart(restartButton, false, false, 0);
            navHBox.PackEnd(hintsBox, false, false, 0);

            return navHBox;
        }

        private VBox CreateContentBox()
        {
            boardImage.Drawn += (sender, args) =>
            {
                Context context = args.Cr;

                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                context.Translate(xOffset, yOffset);
                context.Scale(minDimention, minDimention);

                gameManager?.DrawBoard(context);
            };
            boardImage.AddEvents((int)EventMask.ButtonPressMask);
            boardImage.ButtonPressEvent += BoardImageClickHandler;
            boardImage.Show();

            HBox boardHBox = new();
            boardHBox.PackStart(boardImage, true, true, 0);
            boardHBox.Show();

            gameTitle.Show();

            VBox contentVBox = new();
            contentVBox.PackStart(gameTitle, false, false, 0);
            contentVBox.PackStart(boardHBox, true, true, 0);
            contentVBox.Show();

            return contentVBox;
        }

        private CancellationToken ResetToken()
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }

        private void LoadDllFiles()
        {
            string appdataPath = GetAppdataPath();
            if (Directory.Exists(appdataPath))
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
            if (windowState != WindowState.ProcessMovement)
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

        private void PerformMovement()
        {
            (GameResult gameResult, bool isActionPerformed) = gameManager!.HandleMovement(x, y);
            Application.Invoke(delegate { boardImage.QueueDraw(); });
            if (isActionPerformed && gameResult == GameResult.InProgress)
            {
                CancellationToken token = ResetToken();
                Player opponent = gameManager!.CurrentPlayer();
                if (gameManager.HumanPlayers[opponent])
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
            GameResult gameResult = gameManager!.HandleAiMovement();
            Application.Invoke(delegate { boardImage.QueueDraw(); });
            if (gameResult == GameResult.InProgress)
            {
                Player opponent = gameManager!.CurrentPlayer();
                if (gameManager.HumanPlayers[opponent])
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
            gameManager!.ComputeHints(token);
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
    }
}
