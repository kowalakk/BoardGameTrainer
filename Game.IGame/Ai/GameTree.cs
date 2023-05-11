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

        public void SelectChildNode(Action action)
        {
            foreach (Node<Action, State> child in SelectedNode.ExpandedChildren)
            {
                if (action!.Equals(child.CorespondingAction))
                {
                    SelectedNode = child;
                    return;
                }
            }
            throw new ArgumentException();
        }

        public void SelectParentNode()
        {
            if (SelectedNode.Parent is not null)
            {
                SelectedNode = SelectedNode.Parent;
                return;
            }
        }
    }
}