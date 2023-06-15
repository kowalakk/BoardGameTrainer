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

        //refactor
        DrawingArea boardImage;
        double x;
        double y;
        //refactor
        public IGameManager? GameManager { get; set; } = null;
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
                WindowState = WindowState.Idle;
                EventsQueue.Add(delegate { GameManager?.Restart(); });

                if (!ConfigWindow.HumanPlayers[Player.One])
                    StartGameByAi();
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

                GameManager?.DrawBoard(context, ConfigWindow.HintsForPlayer(GameManager!.CurrentPlayer()));
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

        public void StartGameByAi()
        {
            WindowState = WindowState.ProcessMovement;
            EventsQueue.Add(PerformAiMovement);
        }

        private void BoardImageClickHandler(object sender, ButtonPressEventArgs args)
        {
            if (WindowState != WindowState.ProcessMovement)
            {
                WindowState = WindowState.ProcessMovement;
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
            (GameResult gameResult, bool isActionPerformed) = GameManager!.HandleMovement(x, y);
            Gtk.Application.Invoke(delegate { boardImage.QueueDraw(); });
            if (isActionPerformed && gameResult == GameResult.InProgress)
            {
                CancellationToken token = ResetToken();
                Player opponent = GameManager!.CurrentPlayer();
                if (ConfigWindow.HumanPlayers[opponent])
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
            Gtk.Application.Invoke(delegate { boardImage.QueueDraw(); });
            if (gameResult == GameResult.InProgress)
            {
                Player opponent = GameManager!.CurrentPlayer();
                if (ConfigWindow.HumanPlayers[opponent])
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
            Gtk.Application.Invoke(delegate
            {
                boardImage.QueueDraw();
                WindowState = WindowState.Idle;
            });
        }

        private void ThreadHandleEvents()
        {
            Action job;
            while (true)
            {
                job = EventsQueue.Take();
                job();
            }
        }

        private CancellationToken ResetToken()
        {
            TokenSource.Cancel();
            TokenSource = new CancellationTokenSource();
            return TokenSource.Token;
        }
    }
}
