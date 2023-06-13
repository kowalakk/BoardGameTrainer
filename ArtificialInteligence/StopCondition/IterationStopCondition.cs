using Game.IGame;

namespace Ai
{
    public class IterationStopCondition : IStopCondition
    {
        private int MaxIterations { get; set; }
        private int LeftIterations { get; set; }
        public IterationStopCondition(int iterations)
        {
            MaxIterations = LeftIterations = iterations;
        }
        public void Start()
        {
            LeftIterations = MaxIterations;
        }
        public void Advance()
        {
            LeftIterations--;
        }
        public bool Occured()
        {
            return LeftIterations <= 0;
        }
    }
}
