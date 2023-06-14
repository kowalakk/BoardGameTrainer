using Ai;
using Game.Checkers;
using Game.IGame;
using Game.Othello;
using Gtk;

namespace BoardGameTrainer
{
    internal class ConfigWindow : Window
    {
        public Dictionary<string, IGameManagerFactory> GameFactories { get; } = new()
        {
            { "Checkers", new CheckersManagerFactory() },
            { "Othello", new OthelloManagerFactory() }
        };

        public Dictionary<string, IAiFactory> AiFactories { get; } = new()
        {
            { "Upper Confidence Bounds for Trees", new UctFactory(1.41) },
            { "Nested Monte Carlo Search", new NmcsFactory(3) }
        };

        public Dictionary<string, IStopConditionFactory> StopConditions { get; } = new()
        {
            { "Time limit", new TimeStopConditionFactory() },
            { "Iterations limit", new IterationStopConditionFactory() }
        };

        public ConfigWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            application.CurrentManagerFactory = GameFactories.FirstOrDefault().Value;
            application.CurrentAiFactory = AiFactories.FirstOrDefault().Value;
            application.CurrentStopConditionFactory = StopConditions.FirstOrDefault().Value;
            application.StopConditionParam = 1000;

            var configWindow = new Gtk.Window(Gtk.WindowType.Toplevel);

            configWindow.Show();
            var contentVbox = new Gtk.VBox();

            DropDownFrame<IGameManagerFactory> gamesFrame = new("Game", GameFactories);
            gamesFrame.Changed += (sender, args) => { application.CurrentManagerFactory = gamesFrame.Active; Console.WriteLine(application.CurrentManagerFactory); };
            gamesFrame.Show();

            DropDownFrame<IAiFactory> aiFrame = new("AI Module", AiFactories);
            aiFrame.Changed += (sender, args) => { application.CurrentAiFactory = aiFrame.Active; };
            aiFrame.Show();

            CheckButtonsFrame numOfPlayersFrame = new("Human players");
            numOfPlayersFrame.FirstClicked += (sender, args) => { application.HumanPlayers[Player.One] = numOfPlayersFrame.FirstActive; };
            numOfPlayersFrame.SecondClicked += (sender, args) => { application.HumanPlayers[Player.Two] = numOfPlayersFrame.SecondActive; };
            numOfPlayersFrame.Show();

            CheckButtonsFrame showHintsFrame = new("Show hints");
            showHintsFrame.FirstClicked += (sender, args) => { application.ShowHints[Player.One] = showHintsFrame.FirstActive; };
            showHintsFrame.SecondClicked += (sender, args) => { application.ShowHints[Player.Two] = showHintsFrame.SecondActive; };
            showHintsFrame.Show();

            StopConditionFrame stopConditionFrame = new(StopConditions);
            stopConditionFrame.Changed += (sender, args) => { application.CurrentStopConditionFactory = stopConditionFrame.Active; };
            stopConditionFrame.ParamChanged += (sender, args) => { application.StopConditionParam = stopConditionFrame.Param; };
            stopConditionFrame.Show();

            Button newGameButton = new("Start new game");
            newGameButton.Clicked += (sender, args) => 
            { 
                application.CreateNewGame();
                configWindow.Close();
            };
            newGameButton.Show();

            contentVbox.PackStart(gamesFrame, false, false, 3);
            contentVbox.PackStart(aiFrame, false, false, 3);
            contentVbox.PackStart(numOfPlayersFrame, false, false, 3);
            contentVbox.PackStart(showHintsFrame, false, false, 3);
            contentVbox.PackStart(stopConditionFrame, false, false, 3);
            contentVbox.PackStart(newGameButton, false, false, 3);
            contentVbox.Show();
            configWindow.Add(contentVbox);
        }

    }

}
