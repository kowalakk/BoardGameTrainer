namespace Game.IGame
{
    public class GameTree<Action, State>
    {
        public Node<Action, State> Root { get; private set; }
        public Node<Action, State> SelectedNode { get; private set; }

        public GameTree(State state)
        {
            Root = new Node<Action, State>(state);
            SelectedNode = Root;
        }

        public bool SelectChildNode(Action action)
        {
            foreach (Node<Action, State> child in SelectedNode.ExpandedChildren)
            {
                if (action!.Equals(child.CorespondingAction))
                {
                    SelectedNode = child;
                    return true;
                }
            }
            return false;
        }

        public bool SelectParentNode()
        {
            if (SelectedNode.Parent is not null)
            {
                SelectedNode = SelectedNode.Parent;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            SelectedNode = Root;
        }
    }
}