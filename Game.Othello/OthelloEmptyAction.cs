namespace Game.Othello
{
    public class OthelloEmptyAction : IOthelloAction
    {
        public override bool Equals(object? other)
        {
            if (other is OthelloEmptyAction)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
