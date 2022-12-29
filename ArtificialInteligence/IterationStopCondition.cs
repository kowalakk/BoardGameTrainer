using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
