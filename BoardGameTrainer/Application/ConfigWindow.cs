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

        public ConfigWindow(MainWindow mainWindow) : base(Gtk.WindowType.Toplevel)
        {
            mainWindow.CurrentManagerFactory = GameFactories.FirstOrDefault().Value;
            mainWindow.CurrentAiFactory = AiFactories.FirstOrDefault().Value;
            mainWindow.CurrentStopConditionFactory = StopConditions.FirstOrDefault().Value;
            mainWindow.StopConditionParam = 1000;

            DropDownFrame<IGameManagerFactory> gamesFrame = new("Game", GameFactories);
            gamesFrame.Changed += (sender, args) => { mainWindow.CurrentManagerFactory = gamesFrame.Active; };
            gamesFrame.Show();

            DropDownFrame<IAiFactory> aiFrame = new("AI Module", AiFactories);
            aiFrame.Changed += (sender, args) => { mainWindow.CurrentAiFactory = aiFrame.Active; };
            aiFrame.Show();

            CheckButtonsFrame numOfPlayersFrame = new("Human players");
            numOfPlayersFrame.FirstClicked += (sender, args) => { mainWindow.HumanPlayers[Player.One] = numOfPlayersFrame.FirstActive; };
            numOfPlayersFrame.SecondClicked += (sender, args) => { mainWindow.HumanPlayers[Player.Two] = numOfPlayersFrame.SecondActive; };
            numOfPlayersFrame.Show();

            CheckButtonsFrame showHintsFrame = new("Show hints");
            showHintsFrame.FirstClicked += (sender, args) => { mainWindow.ShowHints[Player.One] = showHintsFrame.FirstActive; };
            showHintsFrame.SecondClicked += (sender, args) => { mainWindow.ShowHints[Player.Two] = showHintsFrame.SecondActive; };
            showHintsFrame.Show();

            StopConditionFrame stopConditionFrame = new(StopConditions);
            stopConditionFrame.Changed += (sender, args) => { mainWindow.CurrentStopConditionFactory = stopConditionFrame.Active; };
            stopConditionFrame.ParamChanged += (sender, args) => { mainWindow.StopConditionParam = stopConditionFrame.Param; };
            stopConditionFrame.Show();

            Button newGameButton = new("Start new game");
            newGameButton.Clicked += (sender, args) =>
            {
                mainWindow.CreateNewGame();
                Close();
            };
            newGameButton.Show();

            VBox contentVbox = new();
            contentVbox.PackStart(gamesFrame, false, false, 3);
            contentVbox.PackStart(aiFrame, false, false, 3);
            contentVbox.PackStart(numOfPlayersFrame, false, false, 3);
            contentVbox.PackStart(showHintsFrame, false, false, 3);
            contentVbox.PackStart(stopConditionFrame, false, false, 3);
            contentVbox.PackStart(newGameButton, false, false, 3);
            contentVbox.Show();
            Add(contentVbox);
        }

    }

}
