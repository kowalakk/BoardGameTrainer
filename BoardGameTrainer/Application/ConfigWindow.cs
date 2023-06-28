using Ai;
using Game.IGame;
using Game.Othello;
using Gtk;

namespace BoardGameTrainer
{
    internal class ConfigWindow : Window
    {
        public Dictionary<string, IGameManagerFactory> GameFactories { get; } = new()
        {
            { "Othello", new OthelloManagerFactory() }
        };

        private static readonly double uctConstant = 1.41;
        private static readonly int nmcsDepth = 5;
        private static readonly Dictionary<string, IAiFactory> aiFactories = new()
        {
            { "Upper Confidence Bounds for Trees", new UctFactory(uctConstant) },
            { "Nested Monte Carlo Search", new NmcsFactory(nmcsDepth) }
        };
        private static readonly Dictionary<string, IStopConditionFactory> stopConditions = new()
        {
            { "Time limit", new TimeStopConditionFactory() },
            { "Iterations limit", new IterationStopConditionFactory() }
        };
        private static readonly Dictionary<Player, bool> humanPlayers = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };
        private static readonly Dictionary<Player, bool> showHints = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };

        private readonly DropDownFrame<IGameManagerFactory> gameFrame;

        private IGameManagerFactory? currentManagerFactory = null;
        private IAiFactory currentAiFactory = aiFactories.FirstOrDefault().Value;
        private IStopConditionFactory currentStopConditionFactory = stopConditions.FirstOrDefault().Value;
        private int stopConditionParam = 1000;

        public ConfigWindow(MainWindow mainWindow) : base(WindowType.Toplevel)
        {
            currentManagerFactory = GameFactories.FirstOrDefault().Value;

            gameFrame = new("Game", GameFactories, currentManagerFactory);
            gameFrame.Show();

            DropDownFrame<IAiFactory> aiFrame = new("AI Module", aiFactories, currentAiFactory);
            aiFrame.Show();

            PlayersBox playersBox = new(humanPlayers, showHints);
            playersBox.Show();

            StopConditionFrame stopConditionFrame = new(stopConditions, currentStopConditionFactory);
            stopConditionFrame.ParamChanged += (sender, args) => { stopConditionParam = stopConditionFrame.Param; };
            stopConditionFrame.Show();

            Button newGameButton = new("Start new game");
            newGameButton.Clicked += (sender, args) =>
            {
                if (currentManagerFactory is not null)
                {
                    mainWindow.CreateGameManager(
                        currentManagerFactory, 
                        currentAiFactory, 
                        currentStopConditionFactory.Create(stopConditionParam),
                        new Dictionary<Player, bool>(humanPlayers),
                        new Dictionary<Player, bool>(showHints));
                    Hide();
                }
            };
            newGameButton.Show();

            VBox contentVbox = new()
            {
                MarginStart = 3,
                MarginEnd = 3
            };
            contentVbox.PackStart(gameFrame, false, false, 3);
            contentVbox.PackStart(aiFrame, false, false, 3);
            contentVbox.PackStart(playersBox, false, false, 3);
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
        public void UpdateGameFactoryDict(IGameManagerFactory factory)
        {
            GameFactories.Add(factory.Name, factory);
            gameFrame.AddEntry(factory.Name);
        }
    }

}
