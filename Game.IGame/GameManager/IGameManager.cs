using Cairo;

namespace Game.IGame
{
    public interface IGameManager
    {
        public Dictionary<Player, bool> HumanPlayers { get; }
        public Dictionary<Player, bool> ShowHints { get; }
        public int NumberOfHints { get; }
        public void DrawBoard(Context context);
        public (GameResult result, bool actionPerformed) HandleMovement(double x, double y);
        public GameResult HandleAiMovement();
        public void ComputeHints(CancellationToken token);
        public Player CurrentPlayer();
        public void Reset();
    }
}