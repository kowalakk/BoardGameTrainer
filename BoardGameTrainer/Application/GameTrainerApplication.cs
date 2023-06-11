using Ai;
using Game.Checkers;
using Game.IGame;
using Game.Othello;
using Gtk;

namespace BoardGameTrainer
{
    public class GameTrainerApplication : Application
    {
        public IGameManager gameManager = new DefaultGameManager(GameResult.InProgress);
        public string[] games = new string[] { "Checkers", "Othello" };
        public int gameNum = -1;
        public int aiNum = -1;
        public bool isPlayer2Ai = true;
        public bool isAImoduleOne = true;
        public double computationTime;
        public int iterationsNumber = 1000;
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
            IAiFactory aiFactory;
            if (aiNum == 0)
            {
                aiFactory = new UctFactory(1.41);
            }
            else //if (aiNum == 1)
            {
                aiFactory = new NmcsFactory(3);
            }
            if (gameNum == 0)
            {
                gameManager = new CheckersManagerFactory()
                    //.CreateGameManager(new UctFactory(1.41), new IterationStopCondition(iterationsNumber));
                    .CreateGameManager(aiFactory, new IterationStopCondition(iterationsNumber));
            }
            else if (gameNum == 1)
            {
                gameManager = new OthelloManagerFactory()
                    .CreateGameManager(aiFactory, new IterationStopCondition(iterationsNumber));
            }
        }
    }
}