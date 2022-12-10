using System.Text.RegularExpressions;

namespace Game.Checkers
{
    public class MoveAction : CheckersAction
    {
        public override Field End { get; }

        public MoveAction(Field start, Field end) : base(start)
        {
            End = end;
        }

        public override bool Equals(CheckersAction? other)
        {
            if (other == null) return false;
            if (other is not MoveAction) return false;
            MoveAction moveAction = (MoveAction)other;
            if (!moveAction.Start.Equals(Start)) return false;
            if (!moveAction.End.Equals(End)) return false;
            return true;
        }

        public override CheckersState PerformOn(CheckersState state, Piece substituteCapturedWith = Piece.None)
        {
            Piece performer = state.GetPieceAt(Start);
            CheckersState newState = new(state);
            newState.SetPieceAt(Start, Piece.None);
            newState.SetPieceAt(End, performer, true);
            return newState;
        }
    }
}
