using Game.IGame;
using Gdk;
using Gtk;
using System.Reflection.Emit;

namespace BoardGameTrainer
{
    enum WindowState
    {
        Idle, ProcessMovement, ComputeHints
    }
    internal class MainWindow : Gtk.Window
    {
        
        WindowState windowState;
        Gtk.DrawingArea boardImage;
        GameTrainerApplication application;
        double x;
        double y;

        // TODO: refactor
        public MainWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            this.application = application;
            string title = "New Game";
            this.DefaultSize = new Gdk.Size(700, 500);

            var mainVBox = new Gtk.VBox();
            var panelHbox = new Gtk.HBox();
            var titleAndContentVBox = new Gtk.VBox();

            var contentHBox = new Gtk.HBox();

            boardImage = new Gtk.DrawingArea();
            boardImage.Drawn += (sender, args) =>
            {
                var context = args.Cr;

                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                context.Translate(xOffset, yOffset);
                context.Scale(minDimention, minDimention);

                application.gameManager.DrawBoard(context, application.numberOfHints);

            };

            boardImage.AddEvents((int)EventMask.ButtonPressMask);
            boardImage.ButtonPressEvent += BoardImageClickHandler;

            contentHBox.PackStart(boardImage, true, true, 0);
            boardImage.Show();

            var newGameButton = new Button("New Game");
            newGameButton.Clicked += (s, e) => { application.AddWindow(new ConfigWindow(application)); };
            var restartButton = new Button("Restart");
            restartButton.Clicked += (s, e) => { application.CreateNewGame(); };
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);

            var mainTitle = new Gtk.Label(title);
            titleAndContentVBox.PackStart(mainTitle, false, false, 0);
            titleAndContentVBox.PackStart(contentHBox, true, true, 0);
            mainTitle.Show();
            contentHBox.Show();

            mainVBox.PackStart(panelHbox, false, false, 0);
            panelHbox.Show();
            mainVBox.PackStart(titleAndContentVBox, true, true, 0);
            titleAndContentVBox.Show();
            this.Add(mainVBox);
            mainVBox.Show();
            newGameButton.Show();
            restartButton.Show();          
        }

        private void BoardImageClickHandler(object sender, ButtonPressEventArgs args)
        {
            if(windowState != WindowState.ProcessMovement)
            {
                windowState = WindowState.ProcessMovement;
                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                x = (args.Event.X - xOffset) / minDimention;
                y = (args.Event.Y - yOffset) / minDimention;
                Console.WriteLine($"Button Pressed at {x}, {y}");

                Thread thr = new Thread(new ThreadStart(ThreadComputation));
                thr.Start();
            }    
        }

        private void ThreadComputation()
        {
            GameResult gameResult = application.gameManager.HandleMovement(x, y, application.isPlayer2Ai);
            if (gameResult != GameResult.InProgress)
                application.gameManager = new DefaultGameManager(gameResult);
            Gtk.Application.Invoke(delegate {
                boardImage.QueueDraw();
                windowState = WindowState.ComputeHints;
                Thread computeHintsThread = new Thread(new ThreadStart(ThreadComputeHints));
                computeHintsThread.Start();
            });
        }

        private void ThreadComputeHints()
        {
            application.gameManager.ComputeHints();
            Gtk.Application.Invoke(delegate
            {
                boardImage.QueueDraw();
                windowState = WindowState.Idle;
            });
        }
    }
}
