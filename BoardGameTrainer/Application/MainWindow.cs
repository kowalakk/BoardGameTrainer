using Cairo;
using Game.IGame;
using Gdk;
using Gtk;
using System.Collections.Concurrent;
using System.Reflection;
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
        private DrawingArea BoardImage { get; } = new DrawingArea();
        private double X { get; set; }
        private double Y { get; set; }
        private ConfigWindow ConfigWindow { get; }
        private Thread HandleEventThread { get; }
        private CancellationTokenSource TokenSource { get; set; } = new();
        private WindowState WindowState { get; set; } = WindowState.Idle;
        private BlockingCollection<Action> EventsQueue { get; } = new(new ConcurrentQueue<Action>());
        public MainWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            DefaultSize = new Gdk.Size(700, 500);

            HandleEventThread = new Thread(new ThreadStart(ThreadHandleEvents));
            HandleEventThread.Start();

            ConfigWindow = new(this);
            DeleteEvent += (sender, args) => ConfigWindow.Dispose();
            application.AddWindow(ConfigWindow);

            Button newGameButton = new("New Game");
            newGameButton.Clicked += (s, e) =>
            {
                ConfigWindow.Show();
            };
            newGameButton.Show();

            Button restartButton = new("Restart");
            restartButton.Clicked += (s, e) =>
            {
                if (GameManager is not null)
                {
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
                    Name = ".dll",
                };
                filter.AddPattern("*.dll");
                dialog.AddFilter(filter);

                if (dialog.Run() == (int)ResponseType.Accept)
                {
                    Assembly assembly = Assembly.LoadFile(dialog.Filename);
                    try
                    {
                        LoadGameFactory(assembly);
                    }
                    catch { } // TODO: okenko z errorem
                }
                dialog.Destroy();
            };
            addGameButton.Show();

            HBox panelHbox = new();
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);
            panelHbox.PackStart(addGameButton, false, false, 0);
            panelHbox.Show();

            BoardImage.Drawn += (sender, args) =>
            {
                Context context = args.Cr;

                int minDimention = Math.Min(BoardImage.AllocatedWidth, BoardImage.AllocatedHeight);
                int xOffset = (BoardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (BoardImage.AllocatedHeight - minDimention) / 2;
                context.Translate(xOffset, yOffset);
                context.Scale(minDimention, minDimention);

                GameManager?.DrawBoard(context);
            };
            BoardImage.AddEvents((int)EventMask.ButtonPressMask);
            BoardImage.ButtonPressEvent += BoardImageClickHandler;
            BoardImage.Show();

            HBox contentHBox = new();
            contentHBox.PackStart(BoardImage, true, true, 0);
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

        private void BoardImageClickHandler(object sender, ButtonPressEventArgs args)
        {
            if (WindowState != WindowState.ProcessMovement)
            {
                WindowState = WindowState.ProcessMovement;
                int minDimention = Math.Min(BoardImage.AllocatedWidth, BoardImage.AllocatedHeight);
                int xOffset = (BoardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (BoardImage.AllocatedHeight - minDimention) / 2;
                X = (args.Event.X - xOffset) / minDimention;
                Y = (args.Event.Y - yOffset) / minDimention;

                PerformMovement();
            }
        }

        internal void StartGame()
        {
            ResetToken();
            WindowState = WindowState.Idle;
            if (GameManager!.HumanPlayers[Player.One])
            {
                WindowState = WindowState.ComputeHints;
                EventsQueue.Add(ComputeHints);
            }
            else
            {
                WindowState = WindowState.ProcessMovement;
                EventsQueue.Add(PerformAiMovement);
            }
        }

        private void PerformMovement()
        {
            (GameResult gameResult, bool isActionPerformed) = GameManager!.HandleMovement(X, Y);
            Application.Invoke(delegate { BoardImage.QueueDraw(); });
            if (isActionPerformed && gameResult == GameResult.InProgress)
            {
                CancellationToken token = ResetToken();
                Player opponent = GameManager!.CurrentPlayer();
                if (GameManager.HumanPlayers[opponent])
                {
                    WindowState = WindowState.ComputeHints;
                    EventsQueue.Add(ComputeHints);
                }
                else
                {
                    EventsQueue.Add(PerformAiMovement);
                }
            }
            else
            {
                WindowState = WindowState.Idle;
            }
        }

        private void PerformAiMovement()
        {
            GameResult gameResult = GameManager!.HandleAiMovement();
            Application.Invoke(delegate { BoardImage.QueueDraw(); });
            if (gameResult == GameResult.InProgress)
            {
                Player opponent = GameManager!.CurrentPlayer();
                if (GameManager.HumanPlayers[opponent])
                {
                    WindowState = WindowState.ComputeHints;
                    EventsQueue.Add(ComputeHints);
                }
                else
                {
                    EventsQueue.Add(PerformAiMovement);
                }
            }
            else
            {
                WindowState = WindowState.Idle;
            }
        }

        private void ComputeHints()
        {
            CancellationToken token = ResetToken();
            GameManager!.ComputeHints(token);
            Application.Invoke(delegate
            {
                BoardImage.QueueDraw();
                WindowState = WindowState.Idle;
            });
        }

        private void ThreadHandleEvents()
        {
            while (true)
            {
                Action job = EventsQueue.Take();
                job();
            }
        }

        private CancellationToken ResetToken()
        {
            TokenSource.Cancel();
            TokenSource = new CancellationTokenSource();
            return TokenSource.Token;
        }

        private IGameManagerFactory LoadGameFactory(Assembly asm)
        {
            foreach (Type type in asm.GetTypes())
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
