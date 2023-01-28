using Game.IGame;
using Gdk;
using Gtk;

namespace BoardGameTrainer
{
    internal class MainWindow : Gtk.Window
    {
        // TODO: refactor
        public MainWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            string title = "New Game";
            this.DefaultSize = new Gdk.Size(700, 500);

            var mainVBox = new Gtk.VBox();
            var panelHbox = new Gtk.HBox();
            var titleAndContentVBox = new Gtk.VBox();

            var contentHBox = new Gtk.HBox();

            var boardImage = new Gtk.DrawingArea();
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
            boardImage.ButtonPressEvent += delegate (object sender, ButtonPressEventArgs args)
            {
                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                double x = (args.Event.X - xOffset) / minDimention;
                double y = (args.Event.Y - yOffset) / minDimention;
                Console.WriteLine($"Button Pressed at {x}, {y}");

                GameResult gameResult = application.gameManager.HandleInput(x, y, application.isPlayer2Ai);
                if (gameResult != GameResult.InProgress)
                    application.gameManager = new DefaultGameManager(gameResult);
                boardImage.QueueDraw();
            };

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
    }
}
