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
        private static double uctConstant = 1.41;
        private static int nmcsDepth = 5;
        private static Dictionary<string, IAiFactory> AiFactories { get; } = new()
        {
            { "Upper Confidence Bounds for Trees", new UctFactory(uctConstant) },
            { "Nested Monte Carlo Search", new NmcsFactory(nmcsDepth) }
        };
        private static Dictionary<string, IStopConditionFactory> StopConditions { get; } = new()
        {
            { "Time limit", new TimeStopConditionFactory() },
            { "Iterations limit", new IterationStopConditionFactory() }
        };
        private static Dictionary<Player, bool> HumanPlayers { get; } = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };
        private static Dictionary<Player, bool> ShowHints { get; } = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };
        private IGameManagerFactory? CurrentManagerFactory { get; set; } = null;
        private IAiFactory CurrentAiFactory { get; set; } = AiFactories.FirstOrDefault().Value;
        private IStopConditionFactory CurrentStopConditionFactory { get; set; } = StopConditions.FirstOrDefault().Value;
        private int StopConditionParam { get; set; } = 1000;
        private int NumberOfHints { get; set; } = int.MaxValue;

        public ConfigWindow(MainWindow mainWindow) : base(WindowType.Toplevel)
        {
            CurrentManagerFactory = GameFactories.FirstOrDefault().Value;

            DropDownFrame<IGameManagerFactory> gamesFrame = new("Game", GameFactories);
            gamesFrame.Changed += (sender, args) => { CurrentManagerFactory = gamesFrame.Active; };
            gamesFrame.Show();

            DropDownFrame<IAiFactory> aiFrame = new("AI Module", AiFactories);
            aiFrame.Changed += (sender, args) => { CurrentAiFactory = aiFrame.Active; };
            aiFrame.Show();

            CheckButtonsFrame numOfPlayersFrame = new("Human players");
            numOfPlayersFrame.FirstClicked += (sender, args) => { HumanPlayers[Player.One] = numOfPlayersFrame.FirstActive; };
            numOfPlayersFrame.SecondClicked += (sender, args) => { HumanPlayers[Player.Two] = numOfPlayersFrame.SecondActive; };
            numOfPlayersFrame.Show();

            CheckButtonsFrame showHintsFrame = new("Show hints");
            showHintsFrame.FirstClicked += (sender, args) => { ShowHints[Player.One] = showHintsFrame.FirstActive; };
            showHintsFrame.SecondClicked += (sender, args) => { ShowHints[Player.Two] = showHintsFrame.SecondActive; };
            showHintsFrame.Show();

            StopConditionFrame stopConditionFrame = new(StopConditions);
            stopConditionFrame.Changed += (sender, args) => { CurrentStopConditionFactory = stopConditionFrame.Active; };
            stopConditionFrame.ParamChanged += (sender, args) => { StopConditionParam = stopConditionFrame.Param; };
            stopConditionFrame.Show();

            Button newGameButton = new("Start new game");
            newGameButton.Clicked += (sender, args) =>
            {
                if (CurrentManagerFactory is not null)
                {
                    mainWindow.GameManager = CurrentManagerFactory
                        .Create(
                            CurrentAiFactory,
                            CurrentStopConditionFactory.Create(StopConditionParam),
                            new Dictionary<Player, bool>(HumanPlayers),
                            new Dictionary<Player, bool>(ShowHints),
                            NumberOfHints);

                    mainWindow.StartGame();
                    Hide();
                }
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

            DeleteEvent += (sender, args) =>
            {
                args.RetVal = true; // prevents closing
                Hide();
            };
        }
    }

}
