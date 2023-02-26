namespace Ai
{
    public class GameTree<Action, State>
        where State : IEquatable<State>
    {
        public Node<Action, State> Root { get; private set; }
        public Node<Action, State> CurrentNode { get; private set; }

        public GameTree(State state)
        {
            Root = new Node<Action, State>(state);
            CurrentNode = Root;
        }

        public void Reset()
        {
            CurrentNode = Root;
        }

        public bool ChooseChildNode(State state)
        {
            foreach (Node<Action, State> child in CurrentNode.ExpandedChildren)
            {
                if (state.Equals(child.CorespondingState))
                {
                    CurrentNode = child;
                    return true;
                }
            }
            return false;
        }

        public bool ChooseParentNode()
        {
            if (CurrentNode.Parent is not null)
            {
                CurrentNode = CurrentNode.Parent;
                return true;
            }
            return false;
        }
    }
}