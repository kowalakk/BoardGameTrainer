using Cairo;

namespace Game.IGame
{
    public interface IGameManager
    {
        public void DrawBoard(Context context, Dictionary<Player, bool> showHints, int numberOfHints);
        public (GameResult result, bool actionPerformed) HandleMovement(double x, double y);
        public void ComputeHints(CancellationToken token);
        public GameResult HandleAiMovement();
        public Player CurrentPlayer();
        public void Restart();
    }
}