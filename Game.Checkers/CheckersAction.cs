using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Game.Checkers
{
    public record Field(int Col, int Row);

    public abstract class CheckersAction : IEquatable<CheckersAction>
    {
        public Field Start { get; protected set; }
        public abstract Field Finnish { get; }

        public CheckersAction(Field start)
        {
            Start = start;
        }

        public abstract bool Equals(CheckersAction? other);

        public abstract CheckersState PerformOn(CheckersState state, Piece substituteCapturedWith = Piece.None);

        public override bool Equals(object? obj)
        {
            return Equals(obj as CheckersAction);
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ Finnish.GetHashCode();
        }
    }
}