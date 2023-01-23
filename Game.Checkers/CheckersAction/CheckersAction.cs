using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Game.Checkers
{
    public abstract class CheckersAction : IEquatable<CheckersAction>
    {
        public virtual Field Start { get; protected set; }
        public abstract Field Finish { get; }

        public abstract bool Equals(CheckersAction? other);

        public abstract CheckersState PerformOn(CheckersState state);

        public abstract IEnumerable<Field> GetParticipatingFields();

        public abstract IEnumerable<Field> GetPlayableFields();

        public override bool Equals(object? obj)
        {
            return Equals(obj as CheckersAction);
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ Finish.GetHashCode();
        }
    }
}