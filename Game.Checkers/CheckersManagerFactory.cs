using Game.IGame;

namespace Game.Checkers
{
    public class CheckersManagerFactory : GameManagerFactory
    {
        protected override IGameManager MakeGameManager(AiFactory<CheckersAction, CheckersState, ICheckersInputState> aiFactory)
        {
            return new GameManager<CheckersAction, CheckersState, ICheckersInputState>(new Checkers(), aiFactory);
        }
    }
}
