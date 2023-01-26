namespace Game.Checkers
{
    public class DefaultInputState : ICheckersInputState
    {
        public ICheckersAction? PreviousAction { get; }
        public DefaultInputState(ICheckersAction? previousAction = null)
        {
            PreviousAction = previousAction;
        }
    }
}