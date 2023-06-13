using Game.IGame;
using System.Diagnostics;

namespace Ai
{
    public class TimeStopCondition : IStopCondition
    {
        public Stopwatch Stopwatch { get; set; }

        private int TimeInterval { get; set; }

        public TimeStopCondition(int miliseconds)
        {
            Stopwatch = new();
            TimeInterval = miliseconds;
        }
        public void Start()
        {
            Stopwatch.Restart();
        }
        public void Advance() { }
        public bool Occured()
        {
            return Stopwatch.ElapsedMilliseconds >= TimeInterval;
        }
    }
}
