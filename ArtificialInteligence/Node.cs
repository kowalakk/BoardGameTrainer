namespace ArtificialIntelligence
{
    public record Node<Action, State>
    {
        public Action? CorespondingAction { get; set; }
        public State CorespondingState { get; set; }
        public Node<Action, State>? Parent { get; set; }
        public List<Node<Action, State>> Children { get; set; }
        public long VisitCount { get; set; }
        public long SuccessCount { get; set; }
        public Node(State state)
        {
            CorespondingState = state;
            CorespondingAction = default(Action);
            Children = new List<Node<Action, State>>();
            Parent = null;
            VisitCount = 0;
            SuccessCount = 0;
        }
        public Node(Action action, State state, Node<Action, State>? parent = null)
        {
            CorespondingAction = action;
            CorespondingState = state;
            Children = new List<Node<Action, State>>();
            Parent = parent;
            VisitCount = 0;
            SuccessCount = 0;
        }

    }
}