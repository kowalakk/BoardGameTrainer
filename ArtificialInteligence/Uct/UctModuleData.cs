namespace Ai
{
    public class UctModuleData<Action, State>
        where State : IEquatable<State>
    {
        internal Node<Action, State> Root { get; private set; }
        public UctModuleData(State state)
        {
            Root = new Node<Action, State>(state);
        }
        public void ChangeRootAfterAction(Action action) 
        { 
            foreach(Node<Action, State> child in Root.ExpandedChildren)
            {
                if(action!.Equals(child.CorespondingAction))
                {
                    Root = child;
                    break;
                }
            }
        }
    }
}