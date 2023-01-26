using Cairo;
using Game.IGame;

namespace BoardGameTrainer
{
    internal class DefaultGameManager : IGameManager
    {
        public void DrawBoard(Context context, int bestShownActionsCount) { }

        public GameResult HandleInput(double x, double y, bool isPlayer2Ai)
        {
            return GameResult.InProgress;
        }
    }
}
