namespace ArtificialIntelligence
{
    internal record Node<State>
    {
        public Node(State state, Node<State>? parent = null)
        {
            CorespondingState = state;
            Children = new List<Node<State>>();
            Parent = parent;
            VisitCount = 0;
            SuccessCount = 0;
        }

        public State CorespondingState { get; set; }
        public Node<State>? Parent { get; set; }
        public List<Node<State>> Children { get; set; }
        public long VisitCount { get; set; }
        public long SuccessCount { get; set; }

    }
}