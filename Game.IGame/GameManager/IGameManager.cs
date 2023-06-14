using Cairo;

namespace Game.IGame
{
    public interface IGameManager
    {
        public void DrawBoard(Context context, Dictionary<Player, bool> showHints, int numberOfHints);
        public (GameResult result, bool actionPerformed) HandleMovement(double x, double y, bool isPlayer2Ai);
        public void ComputeHints(CancellationToken token);
        public GameResult PerformOponentsMovement(GameResult gameResult);
        public void Restart();
    }
}