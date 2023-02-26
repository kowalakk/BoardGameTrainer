using Game.IGame;

namespace Ai
{
    public class Uct<Action, State, InputState> : IAi<Action, State, InputState>
    {
        private IStopCondition StopCondition { get; set; }
        private double UCTConstant { get; }
        private IGame<Action, State, InputState> Game { get; }
        public Uct(double uCTConstant, IGame<Action, State, InputState> game, IStopCondition condition)
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

        public Action ChooseAction(State state)
        {
            return MoveAssessment(state).MaxBy(action => { return action.Item2; }).Item1;
        }

        private void UCTSearch(Node<Action, State> root)
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
                $" - {(double)watch.ElapsedMilliseconds/iterations} ms/iteration");
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
        private GameResult DefaultPolicy(State state)
        {
            GameResult gameResult = Game.Result(state);
            while (gameResult == GameResult.InProgress)
            {
                IEnumerable<Action> possibleActions = Game.PossibleActions(state);
                Action randomAction = possibleActions.RandomElement();
                state = Game.PerformAction(randomAction, state);
                gameResult = Game.Result(state);
            }
            return gameResult;
        }
        private void Backup(Node<Action, State> node, GameResult gameResult)
        {
            int delta = -1;
            if (gameResult == GameResult.Draw)
                delta = 0;
            if (gameResult == (GameResult)Game.CurrentPlayer(node.CorespondingState))
                delta = 1;
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
            return (double)node.SuccessCount / node.VisitCount + UCTConstant * Math.Sqrt(2 * Math.Log(node.Parent!.VisitCount) / node.VisitCount);
        }
    }
}
