using Game.Checkers;
using Game.IGame;
using Gdk;
using Gtk;

namespace BoardGameTrainer
{
    public static class Program
    {
        // Dodałem bezparametrowy interfejs IGame. Znalazłem, że tak się robi
        // Dołączyłem też referencję do warcabów jako przykład.
        // Trzeba ją potem usunąć - nasz program powinien przeszukiwać katalog
        // i sam dodawać gry z .dllek, a nie mieć bezpośrednią referencję
        private static IGame game = new Checkers();
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

            // zamiast tego będzie przypisanie do boardImage wartości game.drawBoard()
            var boardImage = new Gtk.Image(boardPixbuf);
            contentHBox.PackStart(boardImage, false, false, 0);
            boardImage.Show();

            var newGameButton = new Button("New Game");
            newGameButton.Clicked += (s, e) => OpenConfigWindow();
            var restartButton = new Button("Restart");
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);

            var mainTitle = new Gtk.Label(title);
            titleAndContentVBox.PackStart(mainTitle, false, false, 0);
            titleAndContentVBox.PackStart(contentHBox, false, false, 0);
            mainTitle.Show();
            contentHBox.Show();

            mainVBox.PackStart(panelHbox, false, false, 0);
            panelHbox.Show();
            mainVBox.PackStart(titleAndContentVBox, false, false, 0);
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
            var gameLabel = new Gtk.Label("Game");
            gameHBox.PackStart(gameLabel, false, false, 3);
            gameHBox.PackStart(gamesDropDown, false, false, 3);
            gameLabel.Show();
            gamesDropDown.Show();
            gameHBox.Show();

            var numOfPlayersHbox = new Gtk.HBox();
            var onePlayerRadio = new Gtk.RadioButton((RadioButton) null);
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
