using ArtificialInteligence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialIntelligence
{
    internal class NMCS<State> : IArtificialIntelligence<State, Action, NMCSModuleData>
    {
        public Action ChooseMove(IGame game, State state)
        {
            throw new NotImplementedException();
        }

        public List<(Action, double)> MoveAssessment(IGame game, State state, NMCSModuleData moduleData)
        {
            throw new NotImplementedException();
        }

        void NMCSearch(Node<State> root, int level, int timeInterval)
        {
            throw new NotImplementedException();
        }
        List<Action> Nested(Node<State> node, int level)
        {
            throw new NotImplementedException();
        }
    }
}
