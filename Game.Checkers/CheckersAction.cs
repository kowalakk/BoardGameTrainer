using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Game.Checkers
{
    public struct Field
    {
        public int X { get; }
        public int Y { get; }
        public Piece OccupiedBy { get; set; }
        public Field(int x, int y, Piece piece)
        {
            X = x;
            Y = y;
            OccupiedBy = piece;
        }
    }
    public class CheckersAction : IEquatable<CheckersAction>
    {
        public List<Field> ConsecutiveFields { get; private set; }

        public CheckersAction(params Field[] consecutiveFields) : this(new List<Field>(consecutiveFields)) { }

        public CheckersAction(IEnumerable<Field> consecutiveFields)
        {
            ConsecutiveFields = new List<Field>(consecutiveFields);
            // finnished action on the last line - crowning
            Field lastField = consecutiveFields.Last();
            if (lastField.OccupiedBy == Piece.WhitePawn && lastField.Y == CheckersState.BOARD_ROWS - 1)
            {
                ConsecutiveFields.RemoveAt(ConsecutiveFields.Count - 1);
                lastField.OccupiedBy = Piece.WhiteCrowned;
                ConsecutiveFields.Add(lastField);
            }
            else if (lastField.OccupiedBy == Piece.BlackPawn && lastField.Y == 0)
            {
                ConsecutiveFields.RemoveAt(ConsecutiveFields.Count - 1);
                lastField.OccupiedBy = Piece.BlackCrowned;
                ConsecutiveFields.Add(lastField);
            }
        }

        public bool Equals(CheckersAction? other)
        {
            if (other == null) return false;
            if (Enumerable.SequenceEqual(ConsecutiveFields, other.ConsecutiveFields)) return true;
            return false;
        }
    }
}