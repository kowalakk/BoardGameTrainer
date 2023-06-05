using Game.IGame;

namespace Ai
{
    public class Uct<Action, State, InputState> : Ai<Action, State, InputState>
    {
        private double UctConstant { get; }

        public Uct(double uctConstant, IGame<Action, State, InputState> game, IStopCondition stopCondition)
            : base(game, stopCondition)
        {
            UctConstant = uctConstant;
        }

        public override List<(Action, double)> MoveAssessment(GameTree<Action, State> gameTree)
        {
            UctSearch(gameTree.SelectedNode);
            return gameTree.SelectedNode.ExpandedChildren
                .Select(child => (child.CorespondingAction!, -(double)child.SuccessCount / child.VisitCount))
                .ToList();
        }

        private void UctSearch(Node<Action, State> root)
        {
            var watch = new System.Diagnostics.Stopwatch();
            int iterations = 0;
            watch.Start();

            while (!StopCondition.StopConditionOccured())
            {
                Node<Action, State> node = TreePolicy(root);
                GameResult gameResult = DefaultPolicy(node.CorespondingState);
                Backup(node, gameResult);
                iterations++;
            }

            watch.Stop();
            Console.WriteLine($"UCT search execution time: {watch.ElapsedMilliseconds} ms" +
                $" - {(double)watch.ElapsedMilliseconds / iterations} ms/iteration");
        }

        private Node<Action, State> TreePolicy(Node<Action, State> node)
        {
            if (Game.Result(node.CorespondingState) == GameResult.InProgress)
            {
                Expand(node);
                if (node.UnexpandedChildren!.Any()) // node not fully expanded
                {
                    Node<Action, State> unexpandedChild = node.UnexpandedChildren!.Dequeue();
                    node.ExpandedChildren.Add(unexpandedChild);
                    return unexpandedChild;
                }
                // game is InProgress so a child exists
                return TreePolicy(BestChild(node)!);
            }
            return node;
        }

        private void Expand(Node<Action, State> node)
        {
            if (node.UnexpandedChildren == null)
            {
                IEnumerable<Action> possibleActions = Game.PossibleActions(node.CorespondingState);
                List<Node<Action, State>> unexpandedChildren = new();
                foreach (Action action in possibleActions)
                {
                    State childState = Game.PerformAction(action, node.CorespondingState);
                    Node<Action, State> childNode = new(action, childState, node);
                    unexpandedChildren.Add(childNode);
                }
                unexpandedChildren.Shuffle();
                node.UnexpandedChildren = new Queue<Node<Action, State>>(unexpandedChildren);
            }
        }

        private void Backup(Node<Action, State> node, GameResult gameResult)
        {
            int delta = Delta(node.CorespondingState, gameResult);
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
            return node.ExpandedChildren.MaxBy(ArgMax);
        }

        private double ArgMax(Node<Action, State> node)
        {
            return (double)node.SuccessCount / node.VisitCount + UctConstant * Math.Sqrt(2 * Math.Log(node.Parent!.VisitCount) / node.VisitCount);
        }

        public override void MoveGameToNextState(GameTree<Action, State> gameTree, Action action)
        {
            gameTree.SelectChildNode(action);
        }

        public override void MoveGameToPreviousState(GameTree<Action, State> gameTree, Action action)
        {
            gameTree.SelectParentNode();
        }
    }
}
