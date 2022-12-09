using ArtificialInteligence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArtificialIntelligence
{
    internal class MCTS<Game, State> : IArtificialIntelligence<Game, State, MCTSModuleData>
    {
        public Action ChooseMove(Game game, State state)
        {
            throw new NotImplementedException();
        }

        public List<(Action, double)> MoveAssessment(Game game, State state, MCTSModuleData moduleData)
        {
            throw new NotImplementedException();
        }

        void MCTSearch(Node root, int timeInterval)
        {
            throw new NotImplementedException();
        }
        Node TreePolicy(Node node)
        {
            throw new NotImplementedException();
        }
        Node Expand(Node node, Game game)
        {
            throw new NotImplementedException();
        }
        int DefaultPolicy(State state, Game game)
        {
            throw new NotImplementedException();
        }
        void Backup(Node node, int delta)
        {
            throw new NotImplementedException();
        }
        Node BestChild(Node node)
        {
            throw  new NotImplementedException();
        }
    }
}
