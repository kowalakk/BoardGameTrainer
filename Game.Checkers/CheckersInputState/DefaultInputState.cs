namespace Game.Checkers
{
    public class DefaultInputState : ICheckersInputState
    {
        public bool Equals(ICheckersInputState? other)
        {
            if (other == null) return false;
            return other is DefaultInputState;
        }
    }
}