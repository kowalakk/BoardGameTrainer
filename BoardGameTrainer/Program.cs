using Ai;
using Game.Checkers;
using Game.IGame;
using Game.Othello;
using Gdk;
using Gtk;

namespace BoardGameTrainer
{
    public static class Program
    {

        private static Application app;

        private static IGameManager gameManager = new DefaultGameManager(GameResult.InProgress);
        private static string[] games = new string[] { "Checkers", "Othello" };
        private static int gameNum = -1;
        private static bool showHintsForPlayer1 = true;
        private static bool showHintsForPlayer2 = false;
        private static bool isPlayer2Ai = true;
        private static bool isAImoduleOne = true;
        private static double computationTime;
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            app = new Application("x.y.z", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);
            var mainWindow = createMainWindow();
            app.AddWindow(mainWindow);

            Application.Run();
        }

        public static Gtk.Window createMainWindow()
        {

            string title = "New Game";
            var win = new Gtk.Window(Gtk.WindowType.Toplevel);
            win.DefaultSize = new Gdk.Size(700, 500);

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

                gameManager.DrawBoard(context, int.MaxValue);

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

                GameResult gameResult = gameManager.HandleInput(x, y, isPlayer2Ai);
                if (gameResult != GameResult.InProgress)
                    gameManager = new DefaultGameManager(gameResult);
                boardImage.QueueDraw();
            };

            contentHBox.PackStart(boardImage, true, true, 0);
            boardImage.Show();

            var newGameButton = new Button("New Game");
            newGameButton.Clicked += (s, e) => { OpenConfigWindow(); };
            var restartButton = new Button("Restart");
            restartButton.Clicked += (s, e) => { CreateNewGame(); };
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
            win.Add(mainVBox);
            mainVBox.Show();
            newGameButton.Show();
            restartButton.Show();
            win.Show();
            win.DeleteEvent += (sender, args) => Application.Quit();
            return win;
        }

        static void OpenConfigWindow()
        {
            gameNum= 0;
            var configWindow = new Gtk.Window(Gtk.WindowType.Toplevel);
            app.AddWindow(configWindow);
            configWindow.Show();
            var contentVbox = new Gtk.VBox();

            var gameHBox = new Gtk.HBox();
            var gamesDropDown = new Gtk.ComboBox(games);
            gamesDropDown.Active = 0;
            gamesDropDown.Changed += (sender, args) => { gameNum = gamesDropDown.Active; };
            var gameLabel = new Gtk.Label("Game");
            gameHBox.PackStart(gameLabel, false, false, 3);
            gameHBox.PackStart(gamesDropDown, false, false, 3);
            gameLabel.Show();
            gamesDropDown.Show();
            gameHBox.Show();

            var numOfPlayersHbox = new Gtk.HBox();
            var onePlayerRadio = new Gtk.RadioButton((RadioButton)null);
            onePlayerRadio.Label = "One Player";
            var twoPlayerRadio = new Gtk.RadioButton(onePlayerRadio);
            twoPlayerRadio.Label = "Two Players";
            numOfPlayersHbox.PackStart(onePlayerRadio, false, false, 3);
            numOfPlayersHbox.PackStart(twoPlayerRadio, false, false, 3);
            Frame numOfPlayersFrame = new("Number of players")
            {
                numOfPlayersHbox
            };
            onePlayerRadio.Clicked += (sender, args) => { isPlayer2Ai = true; };
            twoPlayerRadio.Clicked += (sender, args) => { isPlayer2Ai = false; };
            numOfPlayersFrame.Show();
            numOfPlayersHbox.Show();
            onePlayerRadio.Show();
            twoPlayerRadio.Show();

            var showHintsHBox = new Gtk.HBox();
            var hintsForPlayer1Checkbox = new Gtk.CheckButton();
            hintsForPlayer1Checkbox.Active = true;
            hintsForPlayer1Checkbox.Label = "Player 1";
            var hintsForPlayer2Checkbox = new Gtk.CheckButton();
            hintsForPlayer2Checkbox.Label = "Player 2";
            hintsForPlayer1Checkbox.Clicked += (sender, args) => { showHintsForPlayer1 = hintsForPlayer1Checkbox.Active; };
            hintsForPlayer2Checkbox.Clicked += (sender, args) => { showHintsForPlayer2 = hintsForPlayer2Checkbox.Active; };
            showHintsHBox.PackStart(hintsForPlayer1Checkbox, false, false, 3);
            showHintsHBox.PackStart(hintsForPlayer2Checkbox, false, false, 3);
            showHintsHBox.Show();
            hintsForPlayer1Checkbox.Show();
            hintsForPlayer2Checkbox.Show();
            Frame showHintsFrame = new("Show hints")
            {
                showHintsHBox
            };
            showHintsFrame.Show();

            var computationTimeSpinButton = new Gtk.SpinButton(0, 1000, 1);
            computationTimeSpinButton.Changed += (sender, args) => { computationTime = computationTimeSpinButton.Value; };
            var computationTimeLabel = new Gtk.Label("ms");
            var computationTimeHBox = new Gtk.HBox();
            computationTimeHBox.PackStart(computationTimeSpinButton, false, false, 3);
            computationTimeHBox.PackStart(computationTimeLabel, false, false, 3);
            computationTimeHBox.SetChildPacking(computationTimeSpinButton, true, true, 0, PackType.Start);
            computationTimeHBox.SetChildPacking(computationTimeLabel, true, false, 0, PackType.Start);
            Frame computationTimeFrame = new("Computation time")
            {
                computationTimeHBox
            };
            computationTimeSpinButton.Show();
            computationTimeLabel.Show();
            computationTimeHBox.Show();
            computationTimeFrame.Show();

            var newGameButton = new Gtk.Button("Start new game");
            newGameButton.Clicked += (sender, args) => { CreateNewGame(); configWindow.Close(); };
            newGameButton.Show();

            contentVbox.PackStart(gameHBox, false, false, 3);
            contentVbox.PackStart(numOfPlayersFrame, false, false, 3);
            contentVbox.PackStart(showHintsFrame, false, false, 3);
            contentVbox.PackStart(computationTimeFrame, false, false, 3);
            contentVbox.PackStart(newGameButton, false, false, 5);
            contentVbox.Show();
            configWindow.Add(contentVbox);
        }
        private static void CreateNewGame()
        {
            if (gameNum == 0)
            {
                gameManager = new CheckersManagerFactory()
                    .CreateGameManager(new UctFactory(1.41), new IterationStopCondition(1000));
            }
            if (gameNum == 1)
            {
                gameManager = new OthelloManagerFactory()
                    .CreateGameManager(new UctFactory(1.41), new IterationStopCondition(1000));
            }
        }
    }
}
