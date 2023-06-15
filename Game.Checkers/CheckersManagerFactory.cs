using Game.IGame;

namespace Game.Checkers
{
    public class CheckersManagerFactory : IGameManagerFactory
    {
        public IGameManager Create(
            IAiFactory aiFactory,
            IStopCondition stopCondition,
            Dictionary<Player, bool> humanPlayers,
            Dictionary<Player, bool> showHints,
            int numberOfHints)
        {
            return new GameManager<ICheckersAction, CheckersState, ICheckersInputState>(
                new Checkers(),
                aiFactory,
                stopCondition,
                humanPlayers,
                showHints,
                numberOfHints);
        }
    }
}
