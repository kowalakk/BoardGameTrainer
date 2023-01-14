namespace Game.Checkers
{
    public class CheckersInputState : IEquatable<CheckersInputState>
    {
        public List<(int, int)> Fields { get; private set; }

        public CheckersInputState(List<(int, int)> fields = null)
        {
            Fields = fields;
        }
        public bool Equals(CheckersInputState? other)
        {
            if (other == null) return false;
            if (Enumerable.SequenceEqual(Fields, other.Fields)) return true;
            return false;
        }
    }
}