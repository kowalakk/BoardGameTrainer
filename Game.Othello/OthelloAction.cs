namespace Game.Othello
{
    public abstract class OthelloAction : IEquatable<OthelloAction>
    {
        public virtual bool Equals(OthelloAction? other)
        {
            if (this is OthelloEmptyAction && other is OthelloEmptyAction)
                return true;
            return false;
        }
    }
}