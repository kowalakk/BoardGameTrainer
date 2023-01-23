using Ai;
using Game.IGame;

namespace Game.Checkers
{
    public class CheckersManagerFactory : IGameManagerFactory
    {
        public IGameManager GetGameManager(IAiFactory aiFactory)
        {
            return new GameManager<CheckersAction, CheckersState, ICheckersInputState>(new Checkers(), aiFactory);
        }
    }
}
