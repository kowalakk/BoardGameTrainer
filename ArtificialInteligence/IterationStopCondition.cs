namespace AI
{
    public class IterationStopCondition : IStopCondition
    {
        private int Iterations { get; set; }
        public IterationStopCondition(int iterations)
        {
            Iterations= iterations;
        }
        public bool StopConditionOccured()
        {
            return Iterations-- == 0;
        }
    }
}
