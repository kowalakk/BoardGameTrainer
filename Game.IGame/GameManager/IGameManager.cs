using Cairo;

namespace Game.IGame
{
    public interface IGameManager
    {
        public void DrawBoard(Context context, int bestShownActionsCount);
        public GameResult HandleInput(double x, double y);
    }
}