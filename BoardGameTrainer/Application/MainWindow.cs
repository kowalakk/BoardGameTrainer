using Cairo;
using Game.IGame;
using Gdk;
using Gtk;
using System.Collections.Concurrent;
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
        private double x;
        private double y;
        private readonly ConfigWindow configWindow;
        private readonly Thread handleEventThread;
        private CancellationTokenSource tokenSource = new();
        private WindowState windowState = WindowState.Idle;
        private readonly BlockingCollection<Action> eventsQueue = new(new ConcurrentQueue<Action>());
        public MainWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            DefaultSize = new Gdk.Size(700, 500);

            handleEventThread = new Thread(new ThreadStart(ThreadHandleEvents));
            handleEventThread.Start();

            configWindow = new(this);
            DeleteEvent += (sender, args) => configWindow.Dispose();
            application.AddWindow(configWindow);

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

            HBox panelHbox = new();
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);
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

        public CancellationToken ResetToken()
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }
    }
}
