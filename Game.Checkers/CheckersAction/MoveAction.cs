using Game.IGame;
using System.Text.RegularExpressions;

namespace Game.Checkers
{
    public class MoveAction : CheckersAction
    {
        public override Field Finish { get; }

        public MoveAction(Field start, Field finish)
        {
            Start= start;
            Finish = finish;
        }

        public override bool Equals(CheckersAction? other)
        {
            if (other == null) return false;
            if (other is not MoveAction) return false;
            MoveAction moveAction = (MoveAction)other;
            if (!moveAction.Start.Equals(Start)) return false;
            if (!moveAction.Finish.Equals(Finish)) return false;
            return true;
        }

        public override IEnumerable<Field> GetParticipatingFields()
        {
            yield return Start;
            yield return Finish;
        }

        public override IEnumerable<Field> GetPlayableFields()
        {
            yield return Finish;
        }

        public override CheckersState PerformOn(CheckersState state)
        {
            Piece movedPiece = state.GetPieceAt(Start);
            CheckersState newState = new(state);
            newState.SetPieceAt(Start, Piece.None);
            newState.SetPieceAtWithPossiblePromotion(Finish, movedPiece);
            newState.CurrentPlayer = state.CurrentPlayer == Player.One? Player.Two: Player.One;
            return newState;
        }
    }
}
