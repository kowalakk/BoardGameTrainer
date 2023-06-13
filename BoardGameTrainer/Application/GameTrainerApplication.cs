using Ai;
using Game.Checkers;
using Game.IGame;
using Game.Othello;
using Gtk;

namespace BoardGameTrainer
{
    public class GameTrainerApplication : Application
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

        public IGameManagerFactory? CurrentManagerFactory { get; set; } = null;
        public IAiFactory? CurrentAiFactory { get; set; } = null;
        public IGameManager GameManager { get; set; } = new DefaultGameManager(GameResult.InProgress);

        public bool isPlayer2Ai = true;
        public double computationTime;
        public int NumberOfIterations = 1000;
        public (int, int) numberOfHints = (int.MaxValue, int.MaxValue);

        public GameTrainerApplication() : base("x.y.z", GLib.ApplicationFlags.None)
        {
            this.Register(GLib.Cancellable.Current);

            MainWindow mainWindow = new(this);
            mainWindow.Show();
            mainWindow.DeleteEvent += (sender, args) => Quit();

            this.AddWindow(mainWindow);
        }

        public void CreateNewGame()
        {
            if (CurrentManagerFactory is null || CurrentAiFactory is null)
                GameManager = new DefaultGameManager(GameResult.InProgress);
            else
                GameManager = CurrentManagerFactory
                    .CreateGameManager(CurrentAiFactory, new IterationStopCondition(NumberOfIterations));
        }
    }
}