using ArtificialInteligence;
using Game.IGame;

namespace ArtificialIntelligence
{
    public class UCT<Action, State, InputState> : IArtificialIntelligence<Action, State, InputState, UCTModuleData<Action, State>>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        private IStopCondition StopCondition { get; set; }
        private double UCTConstant { get; }
        private IGame<Action, State, InputState> Game { get; }
        public UCT(double uCTConstant, IGame<Action, State, InputState> game, IStopCondition condition)
        {
            UCTConstant = uCTConstant;
            Game = game;
            StopCondition = condition;
        }

        public List<(Action, double)> MoveAssessment(State state)
        {
            Node<Action, State> root = new(state);
            UCTSearch(root);
            return root.ExpandedChildren.Select(child => (child.CorespondingAction!, -(double)child.SuccessCount / child.VisitCount)).ToList();
        }

        public Action ChooseMove(State state)
        {
            return MoveAssessment(state).MaxBy(action => { return action.Item2; }).Item1;
        }
        private void UCTSearch(Node<Action, State> root)
        {
            IEnumerator<Node<Action, State>> treePolicyEnumerator = TreePolicy(root);
            while (!StopCondition.StopConditionOccured())
            {
                treePolicyEnumerator.MoveNext();
                SingleUCTIteration(treePolicyEnumerator);
            }
        }
        private void SingleUCTIteration(IEnumerator<Node<Action, State>> treePolicyEnumerator)
        {
            Node<Action, State> node = treePolicyEnumerator.Current;
            int delta = DefaultPolicy(node.CorespondingState);
            Backup(node, delta);
        }
        private IEnumerator<Node<Action, State>> TreePolicy(Node<Action, State> node)
        {
            GameResults gameResult = Game.GameResult(node.CorespondingState);
            while (gameResult == GameResults.InProgress)
            {
                yield return Expand(node);
                while (node.UnexpandedChildren!.Any()) // node not fully expanded
                {
                    yield return Expand(node);
                }
                node = BestChild(node)!; // game is InProgress so a child exists
                gameResult = Game.GameResult(node.CorespondingState);
            }
        }

        private Node<Action, State> Expand(Node<Action, State> node)
        {
            if (node.UnexpandedChildren == null)
            {
                IEnumerable<Action> possibleActions = Game.PossibleActions(node.CorespondingState);
                List<Node<Action,State>> unexpandedChildren = new();
                foreach (Action action in possibleActions)
                {
                    State childState = Game.PerformAction(action, node.CorespondingState);
                    Node<Action, State> childNode = new(action, childState, node);
                    unexpandedChildren.Add(childNode);
                }
                unexpandedChildren.Shuffle();
                node.UnexpandedChildren = new Queue<Node<Action, State>>(unexpandedChildren);
            }
            Node<Action, State> unexpandedChild = node.UnexpandedChildren.Dequeue();
            node.ExpandedChildren.Add(unexpandedChild);
            return unexpandedChild;
        }
        private int DefaultPolicy(State state)
        {
            GameResults gameResult = Game.GameResult(state);
            while (gameResult == GameResults.InProgress)
            {
                IEnumerable<Action> possibleActions = Game.PossibleActions(state);
                Action randomAction = possibleActions.RandomElement();
                state = Game.PerformAction(randomAction, state);
                gameResult = Game.GameResult(state);
            }
            if (gameResult == GameResults.Draw)
                return 0;
            if (gameResult == (GameResults)Game.CurrentPlayer(state))
                return 1;
            return -1;
        }
        private void Backup(Node<Action, State> node, int delta)
        {
            Node<Action, State>? predecessor = node;
            while (predecessor != null)
            {
                predecessor.VisitCount++;
                predecessor.SuccessCount += delta;
                delta = -delta;
                predecessor = predecessor.Parent;
            }
        }
        private Node<Action, State>? BestChild(Node<Action, State> node)
        {
            Node<Action, State>? bestChild = null;
            double argMax = 0;
            foreach (Node<Action, State> child in node.ExpandedChildren)
            {
                double newArgMax = ArgMax(child);
                if (newArgMax > argMax)
                {
                    bestChild = child;
                    argMax = newArgMax;
                }
            }
            return bestChild;
        }
        private double ArgMax(Node<Action, State> node)
        {
            return (double)node.SuccessCount / node.VisitCount + UCTConstant * Math.Sqrt(2 * Math.Log(node.Parent!.VisitCount) / node.VisitCount);
        }
    }
}
