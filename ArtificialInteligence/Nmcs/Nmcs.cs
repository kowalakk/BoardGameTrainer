using Game.IGame;
using System.Threading;

namespace Ai
{
    public class Nmcs<Action, State, InputState> : Ai<Action, State, InputState>
    {
        private int Depth { get; }

        public Nmcs(int depth, IGame<Action, State, InputState> game, IStopCondition stopCondition)
            : base(game, stopCondition)
        {
            Depth = depth;
        }

        public override List<(Action, double)> MoveAssessment(GameTree<Action, State> gameTree, CancellationToken token)
        {
            List<Node<Action, State>> leaves = new();
            Nesting(Depth, gameTree.SelectedNode, leaves);
            if (leaves.Count > 0)
            {
                NmcSearch(leaves.ToArray(), token);
                Backup(leaves);
            }
            return gameTree.SelectedNode.ExpandedChildren
                .Select(child => (child.CorespondingAction!, -(double)child.SuccessCount / child.VisitCount))
                .ToList();
        }

        private void Nesting(int depth, Node<Action, State> node, List<Node<Action, State>> leaves)
        {
            if (depth == 1)
            {
                leaves.Add(node);
                return;
            }
            if (!node.ExpandedChildren.Any())
            {
                IEnumerable<Action> possibleActions = Game.PossibleActions(node.CorespondingState);
                if (!possibleActions.Any())
                {
                    leaves.Add(node);
                    return;
                }
                foreach (Action action in possibleActions)
                {
                    State childState = Game.PerformAction(action, node.CorespondingState);
                    Node<Action, State> childNode = new(action, childState, node);
                    node.ExpandedChildren.Add(childNode);
                }
            }
            foreach (Node<Action, State> child in node.ExpandedChildren)
            {
                Nesting(depth - 1, child, leaves);
            }
        }

        private void NmcSearch(Node<Action, State>[] leaves, CancellationToken token)
        {
            var watch = new System.Diagnostics.Stopwatch();
            int iterations = 0;
            watch.Start();

            while (!StopCondition.StopConditionOccured() && !token.IsCancellationRequested)
            {
                Node<Action, State> randomLeaf = leaves.RandomElement();
                GameResult gameResult = DefaultPolicy(randomLeaf.CorespondingState);
                int delta = Delta(randomLeaf.CorespondingState, gameResult);
                randomLeaf.VisitCount++;
                randomLeaf.SuccessCount += delta;

                iterations++;//for tests
            }

            watch.Stop();
            Console.WriteLine($"NMC search execution time: {watch.ElapsedMilliseconds} ms" +
                $" - {(double)watch.ElapsedMilliseconds / iterations} ms/iteration");
        }

        private void Backup(List<Node<Action, State>> leaves)
        {
            foreach (Node<Action, State> leaf in leaves)
            {
                Node<Action, State>? predecessor = leaf.Parent;
                long successCount = -leaf.SuccessCount;
                while (predecessor != null)
                {
                    predecessor.VisitCount += leaf.VisitCount;
                    predecessor.SuccessCount += successCount;
                    successCount = -successCount;
                    predecessor = predecessor.Parent;
                }
            }
        }
    }
}
