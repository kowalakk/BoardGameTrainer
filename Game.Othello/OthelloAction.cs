namespace Game.Othello
{
    public abstract class OthelloAction : IEquatable<OthelloAction>
    {
        public abstract bool Equals(OthelloAction? other);
        public override bool Equals(object? obj)
        {
            return Equals(obj as OthelloAction);
        }
    }
}