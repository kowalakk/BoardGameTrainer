namespace ArtificialIntelligence
{
    internal class Node<State>
    {
        public Node()
        {
        }

        public long visitCtr { get; set; }
        public long successCtr { get; set; }
        public State state { get; set; }
        public Node<State> parent { get; set; }
        public List<Node<State>> children { get; set; }

    }
}