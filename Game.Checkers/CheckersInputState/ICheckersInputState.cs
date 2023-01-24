namespace Game.Checkers
{
    public interface ICheckersInputState// : IEquatable<ICheckersInputState> 
    { 
        public ICheckersAction? PreviousAction { get; }
    }
}