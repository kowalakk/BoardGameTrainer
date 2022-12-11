namespace Game.Othello
{
    public abstract class OthelloAction : IEquatable<OthelloAction>
    {
        public virtual bool Equals(OthelloAction? other)
        {
            return false;
        }
    }
}