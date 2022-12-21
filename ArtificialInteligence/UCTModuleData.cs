namespace ArtificialIntelligence
{
    public class UCTModuleData<Action, State>
        where State : IEquatable<State>
    {
        internal Node<Action, State> Root { get; private set; }
        public UCTModuleData(State state)
        {
            Root = new Node<Action, State>(state);
        }
        public void ChangeRootAfterAction(Action action) 
        { 
            foreach(Node<Action, State> child in Root.Children)
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