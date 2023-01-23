namespace Ai
{
    public record Node<Action, State>
    {
        public Action? CorespondingAction { get; set; }
        public State CorespondingState { get; set; }
        public Node<Action, State>? Parent { get; set; }
        public List<Node<Action, State>> ExpandedChildren { get; set; }
        public Queue<Node<Action, State>>? UnexpandedChildren { get; set; }
        public long VisitCount { get; set; }
        public long SuccessCount { get; set; }
        public Node(State state)
        {
            CorespondingState = state;
            CorespondingAction = default;
            ExpandedChildren = new List<Node<Action, State>>();
            UnexpandedChildren = null;
            Parent = null;
            VisitCount = 0;
            SuccessCount = 0;
        }
        public Node(Action action, State state, Node<Action, State>? parent = null)
        {
            CorespondingAction = action;
            CorespondingState = state;
            ExpandedChildren = new List<Node<Action, State>>();
            UnexpandedChildren = null;
            Parent = parent;
            VisitCount = 0;
            SuccessCount = 0;
        }

    }
}