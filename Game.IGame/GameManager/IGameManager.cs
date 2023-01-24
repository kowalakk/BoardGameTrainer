using Cairo;

namespace Game.IGame
{
    public interface IGameManager
    {
        public void DrawBoard(Context context);
        public GameResult HandleInput(double x, double y);
    }
}