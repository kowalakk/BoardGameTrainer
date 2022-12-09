using ArtificialInteligence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialIntelligence
{
    internal class NMCS<Game, State> : IArtificialIntelligence<Game, State, NMCSModuleData>
    {
        public Action ChooseMove(Game game, State state)
        {
            throw new NotImplementedException();
        }

        public List<(Action, double)> MoveAssessment(Game game, State state, NMCSModuleData moduleData)
        {
            throw new NotImplementedException();
        }

        void NMCSearch(Node root, int level, int timeInterval)
        {
            throw new NotImplementedException();
        }
        List<Action> Nested(Node node, int level)
        {
            throw new NotImplementedException();
        }
    }
}
