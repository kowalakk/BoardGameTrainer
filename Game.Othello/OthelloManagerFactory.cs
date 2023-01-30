using Game.IGame;

namespace Game.Othello
{
    public class OthelloManagerFactory : IGameManagerFactory
    {
        public IGameManager CreateGameManager(IAiFactory aiFactory, IStopCondition stopCondition)
        {
            return new GameManager<OthelloAction, OthelloState, LanguageExt.Unit>(new Othello(), aiFactory, stopCondition);
        }
    }
}
