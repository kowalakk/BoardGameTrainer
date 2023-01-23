using Ai;
using Game.IGame;

namespace Game.Checkers
{
    public class CheckersManagerFactory : GameManagerFactory
    {
        protected override IGameManager MakeGameManager()
        {
            return new GameManager<CheckersAction, CheckersState, ICheckersInputState>(new Checkers(), 
                new AiFactory<CheckersAction, CheckersState, ICheckersInputState>());
        }
    }
}
