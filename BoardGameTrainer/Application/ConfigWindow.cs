using Game.IGame;
using Gtk;

namespace BoardGameTrainer
{
    internal class ConfigWindow : Window
    {
        // TODO: refactor
        public ConfigWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            application.CurrentManagerFactory = application.GameFactories.FirstOrDefault().Value;
            application.CurrentAiFactory = application.AiFactories.FirstOrDefault().Value;

            var configWindow = new Gtk.Window(Gtk.WindowType.Toplevel);

            configWindow.Show();
            var contentVbox = new Gtk.VBox();

            DropDownFrame<IGameManagerFactory> gamesFrame = new("Game", application.GameFactories);
            gamesFrame.Changed += (sender, args) => { application.CurrentManagerFactory = gamesFrame.Active; Console.WriteLine(application.CurrentManagerFactory); };
            gamesFrame.Show();

            DropDownFrame<IAiFactory> aiFrame = new("AI Module", application.AiFactories);
            aiFrame.Changed += (sender, args) => { application.CurrentAiFactory = aiFrame.Active; };
            aiFrame.Show();

            NumberOfPlayersFrame numOfPlayersFrame = new();
            numOfPlayersFrame.FirstClicked += (sender, args) => { application.isPlayer2Ai = true; };
            numOfPlayersFrame.SecondClicked += (sender, args) => { application.isPlayer2Ai = false; };
            numOfPlayersFrame.Show();

            ShowHintsFrame showHintsFrame = new();
            showHintsFrame.FirstClicked += (sender, args) => { application.numberOfHints.Item1 = showHintsFrame.FirstActive ? int.MaxValue : 0; };
            showHintsFrame.SecondClicked += (sender, args) => { application.numberOfHints.Item2 = showHintsFrame.SecondActive ? int.MaxValue : 0; };
            showHintsFrame.Show();

            // for later
            //SpinButtonFrame computationTimeFrame = new("Computation time", 10, 10000, 10, computationTime, "ms");
            //computationTimeFrame.Changed += (sender, args) => { computationTime = computationTimeFrame.Value; };
            //computationTimeFrame.Show();

            SpinButtonFrame iterationFrame = new("Number of iterations", 1000, 100000, 100, application.NumberOfIterations, "iterations");
            iterationFrame.Changed += (sender, args) => { application.NumberOfIterations = (int)iterationFrame.Value; };
            iterationFrame.Show();

            Button newGameButton = new("Start new game");
            newGameButton.Show();
            newGameButton.Clicked += (sender, args) => { application.CreateNewGame(); configWindow.Close(); };

            contentVbox.PackStart(gamesFrame, false, false, 3);
            contentVbox.PackStart(aiFrame, false, false, 3);
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
