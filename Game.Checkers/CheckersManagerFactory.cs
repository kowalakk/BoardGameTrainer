using Game.IGame;

namespace Game.Checkers
{
    public class CheckersManagerFactory : IGameManagerFactory
    {
        public IGameManager CreateGameManager(IAiFactory aiFactory)
        {
            return new GameManager<CheckersAction, CheckersState, ICheckersInputState>(new Checkers(), aiFactory);
        }
    }
}
