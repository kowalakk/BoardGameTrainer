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
        public IGameManagerFactory? CurrentManagerFactory { get; set; } = null;
        public IAiFactory? CurrentAiFactory { get; set; } = null;
        public IStopConditionFactory? CurrentStopConditionFactory { get; set; } = null;
        public IGameManager? GameManager { get; set; } = null;
        public int StopConditionParam { get; set; }
        public Dictionary<Player, bool> HumanPlayers { get; } = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };
        public Dictionary<Player, bool> ShowHints { get; } = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };
        public int NumberOfHints { get; set; } = (int.MaxValue);
        CancellationTokenSource tokenSource;
        Thread handleEventThread;
        BlockingCollection<Action> eventsQueue = new(new ConcurrentQueue<Action>());
        WindowState windowState;
        DrawingArea boardImage;
        double x;
        double y;

        public MainWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            DefaultSize = new Gdk.Size(700, 500);

            tokenSource = new CancellationTokenSource();

            handleEventThread = new Thread(new ThreadStart(ThreadHandleEvents));
            handleEventThread.Start();

            Button newGameButton = new("New Game");
            newGameButton.Clicked += (s, e) =>
            {
                ConfigWindow configWindow = new(this);
                configWindow.Destroyed += (s, e) =>
                {
                    if (!HumanPlayers[Player.One])
                        eventsQueue.Add(PerformAiMovement);
                };
                application.AddWindow(configWindow);
                configWindow.Show();
            };
            newGameButton.Show();

            Button restartButton = new("Restart");
            restartButton.Clicked += (s, e) =>
            {
                windowState = WindowState.Idle;
                eventsQueue.Add(RestartGame);
                if (!HumanPlayers[Player.One])
                    eventsQueue.Add(PerformAiMovement);
            };
            restartButton.Show();

            HBox panelHbox = new();
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);
            panelHbox.Show();

            boardImage = new DrawingArea();
            boardImage.Drawn += (sender, args) =>
            {
                Context context = args.Cr;

                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                context.Translate(xOffset, yOffset);
                context.Scale(minDimention, minDimention);

                GameManager?.DrawBoard(context, ShowHintsForPlayer() ? NumberOfHints : 0);
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
        public void CreateNewGame()
        {
            if (CurrentManagerFactory is not null
                && CurrentAiFactory is not null
                && CurrentStopConditionFactory is not null)
            {
                GameManager = CurrentManagerFactory
                    .Create(CurrentAiFactory, CurrentStopConditionFactory.Create(StopConditionParam));
            }
        }

        internal void RestartGame()
        {
            GameManager?.Restart();
        }

        internal bool ShowHintsForPlayer()
        {
            return HumanPlayers[GameManager!.CurrentPlayer()] && ShowHints[GameManager.CurrentPlayer()];
        }

        private void PerformMovement()
        {
            (GameResult gameResult, bool isActionPerformed) = GameManager!.HandleMovement(x, y);
            Gtk.Application.Invoke(delegate { boardImage.QueueDraw(); });
            if (isActionPerformed && gameResult == GameResult.InProgress)
            {
                CancellationToken token = ResetToken();
                Player opponent = GameManager!.CurrentPlayer();
                if (HumanPlayers[opponent])
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
            Gtk.Application.Invoke(delegate { boardImage.QueueDraw(); });
            if (gameResult == GameResult.InProgress)
            {
                Player opponent = GameManager!.CurrentPlayer();
                if (HumanPlayers[opponent])
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
            Gtk.Application.Invoke(delegate
            {
                boardImage.QueueDraw();
                windowState = WindowState.Idle;
            });
        }

        private void ThreadHandleEvents()
        {
            Action job;
            while (true)
            {
                job = eventsQueue.Take();
                job();
            }
        }

        private CancellationToken ResetToken()
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }
    }
}
