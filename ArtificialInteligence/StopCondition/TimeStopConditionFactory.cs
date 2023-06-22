using Game.IGame;

namespace Ai
{
    public class TimeStopConditionFactory : IStopConditionFactory
    {
        public IStopCondition Create(int miliseconds)
        {
            return new TimeStopCondition(miliseconds);
        }
    }
}