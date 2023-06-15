using Game.IGame;
using Gtk;

namespace BoardGameTrainer
{/// <summary>
/// ////////////////////////////////////////////////////////GameManager in MainWindow???
/// </summary>
    public class GameTrainerApplication : Application
    {
        public IGameManagerFactory? CurrentManagerFactory { get; set; } = null;
        public IAiFactory? CurrentAiFactory { get; set; } = null;
        public IStopConditionFactory? CurrentStopConditionFactory { get; set; } = null;
        public IGameManager? GameManager { get; set; } = null;
        public int StopConditionParam { get; set; }
        public Dictionary<Player, bool> HumanPlayers { get; } = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };
        public Dictionary<Player, bool> ShowHints { get; } = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };
        public int NumberOfHints { get; set; } = (int.MaxValue);

        public bool isPlayer2Ai = true;

        public GameTrainerApplication() : base("x.y.z", GLib.ApplicationFlags.None)
        {
            Register(GLib.Cancellable.Current);

            MainWindow mainWindow = new(this);
            mainWindow.DeleteEvent += (sender, args) => Quit();
            mainWindow.Show();

            AddWindow(mainWindow);
        }

        public void CreateNewGame()
        {
            if (CurrentManagerFactory is not null
                && CurrentAiFactory is not null
                && CurrentStopConditionFactory is not null)
            {
                GameManager = CurrentManagerFactory
                    .Create(CurrentAiFactory, CurrentStopConditionFactory.Create(StopConditionParam));
            }
        }

        internal void RestartGame()
        {
            GameManager?.Restart();
        }

        internal bool ShowHintsForPlayer()
        {
            return HumanPlayers[GameManager!.CurrentPlayer()] && ShowHints[GameManager.CurrentPlayer()];
        }
    }
}