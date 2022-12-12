using System.Text.RegularExpressions;

namespace Game.Checkers
{
    public class MoveAction : CheckersAction
    {
        public override Field Finnish { get; }

        public MoveAction(Field start, Field finnish) : base(start)
        {
            Finnish = finnish;
        }

        public override bool Equals(CheckersAction? other)
        {
            if (other == null) return false;
            if (other is not MoveAction) return false;
            MoveAction moveAction = (MoveAction)other;
            if (!moveAction.Start.Equals(Start)) return false;
            if (!moveAction.Finnish.Equals(Finnish)) return false;
            return true;
        }

        public override CheckersState PerformOn(CheckersState state, Piece substituteCapturedWith = Piece.None)
        {
            Piece movedPiece = state.GetPieceAt(Start);
            CheckersState newState = new(state);
            newState.SetPieceAt(Start, Piece.None);
            newState.SetPieceAt(Finnish, movedPiece, true);
            return newState;
        }
    }
}
