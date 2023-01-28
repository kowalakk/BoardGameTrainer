namespace Game.Checkers
{
    public interface ICheckersAction : IEquatable<object>
    {
        public abstract Field Start { get; }
        public abstract Field Finish { get; }

        public abstract CheckersState PerformOn(CheckersState state);

        public abstract IEnumerable<Field> GetParticipatingFields();

        public abstract IEnumerable<Field> GetPlayableFields();
    }
}