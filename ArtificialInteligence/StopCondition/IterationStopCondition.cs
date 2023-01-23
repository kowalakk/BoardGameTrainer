namespace Ai
{
    public class IterationStopCondition : IStopCondition
    {
        private int MaxIterations { get; set; }
        private int CurrentIterations { get; set; }
        public IterationStopCondition(int iterations)
        {
            MaxIterations = CurrentIterations = iterations;
        }
        public bool StopConditionOccured()
        {
            //Console.WriteLine($"Iterations left {CurrentIterations}");
            if (CurrentIterations-- == 0)
            {
                CurrentIterations = MaxIterations;
                return true;
            }
            return false;
        }
    }
}
