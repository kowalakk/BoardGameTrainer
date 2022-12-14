using ArtificialInteligence;
using Game.IGame;

namespace ArtificialIntelligence
{
    public class UCT<Action, State> : IArtificialIntelligence<Action, State, UCTModuleData>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        private double UCTConstant { get; }

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
        private IEnumerable<Node<State>> TreePolicy<InputState>(Node<State> node, IGame<Action, State, InputState> game)
        {
            GameResults gameResult = game.GameResult(node.CorespondingState);
            while (gameResult == GameResults.InProgress)
            {
                IEnumerable<Action> possibleActions = game.PossibleActions(node.CorespondingState);
                foreach (Action action in possibleActions)
                {
                    State childState = game.PerformAction(action, node.CorespondingState);
                    Node<State> childNode = new(childState, node);
                    node.Children.Add(childNode);
                    yield return childNode;
                }
                node = BestChild(node)!; // game is InProgress so a child exists
                gameResult = game.GameResult(node.CorespondingState);
            }
        }
        // obsolete?
        private IEnumerable<Node<State>> Expand<InputState>(Node<State> node, IGame<Action, State, InputState> game)
        {
            IEnumerable<Action> possibleActions = game.PossibleActions(node.CorespondingState);
            foreach (Action action in possibleActions)
            {
                State childState = game.PerformAction(action, node.CorespondingState);
                Node<State> childNode = new(childState, node);
                node.Children.Add(childNode);
                yield return childNode;
            }
        }
        private int DefaultPolicy<InputState>(State state, IGame<Action, State, InputState> game)
        {
            GameResults gameResult = game.GameResult(state);
            while (gameResult == GameResults.InProgress)
            {
                IEnumerable<Action> possibleActions = game.PossibleActions(state);
                Action randomAction = possibleActions.RandomElement();
                state = game.PerformAction(randomAction, state);
                gameResult = game.GameResult(state);
            }
            if (gameResult == GameResults.CurrentPlayerWins)
                return 1;
            if (gameResult == GameResults.CurrentOpponentWins)
                return -1;
            return 0;
        }
        private void Backup(Node<State> node, int delta)
        {
            Node<State>? predecessor = node;
            while (predecessor != null)
            {
                predecessor.VisitCount++;
                predecessor.SuccessCount += delta;
                delta = -delta;
                predecessor = predecessor.Parent;
            }
        }
        private Node<State>? BestChild(Node<State> node)
        {
            Node<State>? bestChild = null;
            double argMax = 0;
            foreach (Node<State> child in node.Children)
            {
                double newArgMax = (double)child.SuccessCount / child.VisitCount + UCTConstant * Math.Sqrt(2 * Math.Log(node.VisitCount) / child.VisitCount);
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
