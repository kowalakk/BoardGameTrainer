namespace Game.Checkers
{
    public struct Field
    {
        public int X { get; }
        public int Y { get; }
        public Piece OccupiedBy { get; set; }
        public Field(int x, int y, Piece piece) 
        {
            X = x;
            Y = y;
            OccupiedBy= piece;
        }
    }
    public class CheckersAction : IEquatable<CheckersAction>
    {
        public IEnumerable<Field> ConsecutiveFields { get; private set; }
        public CheckersAction(IEnumerable<Field> consecutiveFields)
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