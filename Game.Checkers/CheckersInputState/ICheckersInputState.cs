namespace Game.Checkers
{
    public interface ICheckersInputState
    { 
        public ICheckersAction? PreviousAction { get; }
    }
}