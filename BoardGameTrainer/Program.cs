using Game.Checkers;
using Game.IGame;
using Gdk;
using Gtk;
using LanguageExt;

namespace BoardGameTrainer
{
    public static class Program
    {
        // Dodałem bezparametrowy interfejs IGame. Znalazłem, że tak się robi
        // Dołączyłem też referencję do warcabów jako przykład.
        // Trzeba ją potem usunąć - nasz program powinien przeszukiwać katalog
        // i sam dodawać gry z .dllek, a nie mieć bezpośrednią referencję
        private static Checkers game = new Checkers();
        private static Application app;
        private static bool isTwoPlayer = false;
        private static bool showHintsForPlayer1 = true;
        private static bool showHintsForPlayer2 = false;
        private static bool isAImoduleOne = true;
        private static int computationTime;
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
            var boardPixbuf = new Gdk.Pixbuf("..\\..\\..\\Tulips.jpg", 200, 200);

            var boardImage = new Gtk.DrawingArea();
            boardImage.Drawn += (sender, args) =>
            {
                var context = args.Cr;

                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                context.Translate(xOffset, yOffset);
                context.Scale(minDimention, minDimention);

                CheckersState state = CheckersState.GetInitialState(); // TODO
                state.SetPieceAt("A1", Piece.WhiteCrowned);
                state.SetPieceAt("H8", Piece.BlackCrowned);
                game.DrawBoard(context, new IdleCIS(), state, new List<(CheckersAction, double)>() { (new CaptureAction(new Field("C3"), new Field("B2"), new Field("A1")),1.0) });

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
                //game.HandleInput(x, y, inputState, state);
            };

            contentHBox.PackStart(boardImage, true, true, 0);
            boardImage.Show();

            var newGameButton = new Button("New Game");
            newGameButton.Clicked += (s, e) => OpenConfigWindow();
            var restartButton = new Button("Restart");
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
            var configWindow = new Gtk.Window(Gtk.WindowType.Toplevel);
            app.AddWindow(configWindow);
            configWindow.Show();
            var contentVbox = new Gtk.VBox();

            var gameHBox = new Gtk.HBox();
            var gamesDropDown = new Gtk.ComboBox(new string[] { "Checkers", "Othello" });
            //gamesDropDown.Changed += ...
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
            onePlayerRadio.Clicked += (sender, args) => { isTwoPlayer = false; };
            twoPlayerRadio.Clicked += (sender, args) => { isTwoPlayer = true; };
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

            Frame computationTimeFrame = new("Computation time")
            {

            };
            computationTimeFrame.Show();

            var newGameButton = new Gtk.Button("Start new game");
            newGameButton.Show();

            contentVbox.PackStart(gameHBox, false, false, 3);
            contentVbox.PackStart(numOfPlayersFrame, false, false, 3);
            contentVbox.PackStart(showHintsFrame, false, false, 3);
            contentVbox.PackStart(computationTimeFrame, false, false, 3);
            contentVbox.PackStart(newGameButton, false, false, 5);
            contentVbox.Show();
            configWindow.Add(contentVbox);
        }

    }
}
