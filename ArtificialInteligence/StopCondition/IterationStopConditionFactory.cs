using Game.IGame;

namespace Ai
{
    public class IterationStopConditionFactory : IStopConditionFactory
    {
        public IStopCondition Create(int iterations)
        {
            return new IterationStopCondition(iterations);
        }
    }
}