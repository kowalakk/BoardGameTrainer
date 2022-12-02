namespace Game.Checkers
{
    public class CheckersAction : IEquatable<CheckersAction>
    {
        public List<(int, int, Piece)> ConsecutiveFields { get; private set; }
        public CheckersAction(List<(int, int, Piece)> consecutiveFields)
        {
            ConsecutiveFields = consecutiveFields;
        }
        public bool Equals(CheckersAction? other)
        {
            if (other == null) return false;
            if (Enumerable.SequenceEqual(ConsecutiveFields, other.ConsecutiveFields)) return true;
            return false;
        }
    }
}