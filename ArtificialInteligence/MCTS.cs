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

        void MCTSearch(Node<State> root, int timeInterval)
        {
            throw new NotImplementedException();
        }
        Node<State> TreePolicy(Node<State> node)
        {
            throw new NotImplementedException();
        }
        Node<State> Expand(Node<State> node, Game game)
        {
            throw new NotImplementedException();
        }
        int DefaultPolicy(State state, Game game)
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
