using ArtificialInteligence;
using Game.IGame;

namespace ArtificialIntelligence
{
    public class UCT<Action, State> : IArtificialIntelligence<Action, State, UCTModuleData>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        private double C { get; }

        public List<(Action, double)> MoveAssessment<InputState>(IGame<Action, State, InputState> game, State state, UCTModuleData moduleData)
        {
            throw new NotImplementedException();
        }

        public Action ChooseMove<InputState>(IGame<Action, State, InputState> game, State state)
        {
            throw new NotImplementedException();
        }
        private void UCTSearch(Node<State> root, IStopCondition condition)
        {
            throw new NotImplementedException();
        }
        private Node<State> TreePolicy(Node<State> node)
        {
            throw new NotImplementedException();
        }
        private Node<State> Expand<InputState>(Node<State> node, IGame<Action, State, InputState> game)
        {
            throw new NotImplementedException();
        }
        private int DefaultPolicy<InputState>(State state, IGame<Action, State, InputState> game)
        {
            throw new NotImplementedException();
        }
        private void Backup(Node<State> node, int delta)
        {
            Node<State>? backer = node;
            while (backer != null)
            {
                backer.VisitCount++;
                backer.SuccessCount += delta;
                backer = backer.Parent;
            }
        }
        private Node<State>? BestChild(Node<State> node)
        {
            Node<State>? bestChild = null;
            double argMax = 0;
            foreach(Node<State> child in node.Children)
            {
                double newArgMax = (double)child.SuccessCount / child.VisitCount + C * Math.Sqrt(2 * Math.Log(node.VisitCount) / child.VisitCount);
                if (newArgMax > argMax)
                {
                    bestChild = child;
                    argMax = newArgMax;
                }
            }
            return bestChild;
        }
    }
}
