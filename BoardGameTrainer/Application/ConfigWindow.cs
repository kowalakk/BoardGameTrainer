using Gtk;

namespace BoardGameTrainer
{
    internal class ConfigWindow : Window
    {
        // TODO: refactor
        public ConfigWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            application.gameNum = 0;
            var configWindow = new Gtk.Window(Gtk.WindowType.Toplevel);

            configWindow.Show();
            var contentVbox = new Gtk.VBox();

            var gameHBox = new Gtk.HBox();
            var gamesDropDown = new Gtk.ComboBox(application.games);
            gamesDropDown.Active = 0;
            gamesDropDown.Changed += (sender, args) => { application.gameNum = gamesDropDown.Active; };
            var gameLabel = new Gtk.Label("Game");
            gameHBox.PackStart(gameLabel, false, false, 3);
            gameHBox.PackStart(gamesDropDown, false, false, 3);
            gameLabel.Show();
            gamesDropDown.Show();
            gameHBox.Show();


            NumberOfPlayersFrame numOfPlayersFrame = new();
            numOfPlayersFrame.Show();
            numOfPlayersFrame.FirstClicked += (sender, args) => { application.isPlayer2Ai = true; };
            numOfPlayersFrame.SecondClicked += (sender, args) => { application.isPlayer2Ai = false; };

            ShowHintsFrame showHintsFrame = new();
            showHintsFrame.Show();
            showHintsFrame.FirstClicked += (sender, args) => { application.showHintsForPlayer1 = showHintsFrame.FirstActive; };
            showHintsFrame.SecondClicked += (sender, args) => { application.showHintsForPlayer2 = showHintsFrame.SecondActive; };

            // for later
            //SpinButtonFrame computationTimeFrame = new("Computation time", 10, 10000, 10, computationTime, "ms");
            //computationTimeFrame.Changed += (sender, args) => { computationTime = computationTimeFrame.Value; };
            //computationTimeFrame.Show();

            SpinButtonFrame iterationFrame = new("Number of iterations", 1000, 100000, 100, application.iterationsNumber, "iterations");
            iterationFrame.Show();
            iterationFrame.Changed += (sender, args) => { application.iterationsNumber = (int)iterationFrame.Value; };

            Button newGameButton = new("Start new game");
            newGameButton.Show();
            newGameButton.Clicked += (sender, args) => { application.CreateNewGame(); configWindow.Close(); };

            contentVbox.PackStart(gameHBox, false, false, 3);
            contentVbox.PackStart(numOfPlayersFrame, false, false, 3);
            contentVbox.PackStart(showHintsFrame, false, false, 3);
            //contentVbox.PackStart(computationTimeFrame, false, false, 3);
            contentVbox.PackStart(iterationFrame, false, false, 3);
            contentVbox.PackStart(newGameButton, false, false, 3);
            contentVbox.Show();
            configWindow.Add(contentVbox);
        }

    }

}
