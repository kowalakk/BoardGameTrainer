namespace Game.Checkers
{
    public interface ICheckersAction : IEquatable<object>
    {
        public abstract int Start { get; }
        public abstract int Finish { get; }

        public abstract CheckersState PerformOn(CheckersState state);

        public abstract IEnumerable<int> GetParticipatingFields();

        public abstract IEnumerable<int> GetPlayableFields();
    }
}