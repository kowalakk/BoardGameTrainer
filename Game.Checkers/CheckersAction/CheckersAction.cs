using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Game.Checkers
{
    public readonly struct Field : IEquatable<Field>
    {
        public int Col { get; }
        public int Row { get; }
        public Field(int col, int row)
        {
            Col = col;
            Row = row;
        }
        /// <summary>
        /// Constructor for writing tests only
        /// </summary>
        /// <param name="field">Field is described by two-letter string (A-H)(1-8)</param>
        public Field(string field)
        {
            Col = field[0] - 'A';
            Row = field[1] - '1';
        }

        public bool Equals(Field other)
        {
            if (Col != other.Col) return false;
            if (Row != other.Row) return false;
            return true;
        }
    }

    public abstract class CheckersAction : IEquatable<CheckersAction>
    {
        public Field Start { get; protected set; }
        public abstract Field Finish { get; }

        public CheckersAction(Field start)
        {
            Start = start;
        }

        public abstract bool Equals(CheckersAction? other);

        public abstract CheckersState PerformOn(CheckersState state);

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