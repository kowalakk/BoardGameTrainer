using ArtificialInteligence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArtificialIntelligence
{
    internal class MCTS<State> : IArtificialIntelligence<State, Action, MCTSModuleData>
    {
        public Action ChooseMove(IGame game, State state)
        {
            throw new NotImplementedException();
        }

        public List<(Action, double)> MoveAssessment(IGame game, State state, MCTSModuleData moduleData)
        {
            throw new NotImplementedException();
        }

        void MCTSearch(Node<State> root, int timeInterval)
        {
            throw new NotImplementedException();
        }
        Node<State> TreePolicy(Node<State> node)
        {
            throw new NotImplementedException();
        }
        Node<State> Expand(Node<State> node, IGame game)
        {
            throw new NotImplementedException();
        }
        int DefaultPolicy(State state, IGame game)
        {
            throw new NotImplementedException();
        }
        void Backup(Node<State> node, int delta)
        {
            throw new NotImplementedException();
        }
        Node<State> BestChild(Node<State> node)
        {
            throw  new NotImplementedException();
        }
    }
}
