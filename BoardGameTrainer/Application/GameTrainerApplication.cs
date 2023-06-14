using Game.IGame;
using Gtk;

namespace BoardGameTrainer
{
    public class GameTrainerApplication : Application
    {
        public IGameManagerFactory? CurrentManagerFactory { get; set; } = null;
        public IAiFactory? CurrentAiFactory { get; set; } = null;
        public IStopConditionFactory? CurrentStopConditionFactory { get; set; } = null;
        public IGameManager GameManager { get; set; } = new DefaultGameManager(GameResult.InProgress);
        public int StopConditionParam { get; set; }

        public bool isPlayer2Ai = true;
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
            if (CurrentManagerFactory is null || CurrentAiFactory is null || CurrentStopConditionFactory is null)
                GameManager = new DefaultGameManager(GameResult.InProgress);
            else
                GameManager = CurrentManagerFactory
                    .Create(CurrentAiFactory, CurrentStopConditionFactory.Create(StopConditionParam));
            Console.WriteLine(StopConditionParam);
        }

        internal void RestartGame()
        {
            GameManager.Restart();
        }
    }
}