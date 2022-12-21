using ArtificialInteligence;
using Game.IGame;
using Gtk;
using System.Collections.Generic;
using static System.Diagnostics.Activity;

namespace ArtificialIntelligence
{
    public class UCT<Action, State, InputState> : IArtificialIntelligence<Action, State, InputState, UCTModuleData<Action, State>>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        private double UCTConstant { get; }
        private IGame<Action, State, InputState> Game { get; }
        public UCT(double uCTConstant, IGame<Action, State, InputState> game)
        {
            UCTConstant = uCTConstant;
            Game = game;
        }

        public List<(Action, double)> MoveAssessment(IGame<Action, State, InputState> game, State state, UCTModuleData<Action, State> moduleData, IStopCondition condition)
        {
            Node<Action, State> root = new(state);
            UCTSearch(root, condition);
            return root.Children.Select(child => (child.CorespondingAction!, ArgMax(child))).ToList();
        }

        public Action ChooseMove(IGame<Action, State, InputState> game, State state)
        {
            throw new NotImplementedException();
        }
        private void UCTSearch(Node<Action, State> root, IStopCondition condition)
        {
            IEnumerator<Node<Action, State>> treePolicyEnumerator = TreePolicy(root);
            while (!condition.StopConditionOccured())
            {
                SingleUCTIteration(treePolicyEnumerator);
                treePolicyEnumerator.MoveNext();
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
            IEnumerator<Node<Action, State>> expandEnumerator = Expand(node);
            GameResults gameResult = Game.GameResult(node.CorespondingState);
            while (gameResult == GameResults.InProgress)
            {
                yield return expandEnumerator.Current;
                while (expandEnumerator.MoveNext()) // node not fully expanded
                {
                    yield return expandEnumerator.Current;
                }
                node = BestChild(node)!; // game is InProgress so a child exists
                gameResult = Game.GameResult(node.CorespondingState);
            }
        }

        private IEnumerator<Node<Action, State>> Expand(Node<Action, State> node)
        {
            IEnumerable<Action> possibleActions = Game.PossibleActions(node.CorespondingState);
            foreach (Action action in possibleActions)
            {
                State childState = Game.PerformAction(action, node.CorespondingState);
                Node<Action, State> childNode = new(action, childState, node);
                node.Children.Add(childNode);
                yield return childNode;
            }
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
            foreach (Node<Action, State> child in node.Children)
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
