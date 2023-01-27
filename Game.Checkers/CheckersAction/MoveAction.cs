using Game.IGame;
using System.Text.RegularExpressions;

namespace Game.Checkers
{
    public class MoveAction : ICheckersAction
    {
        public Field Start { get; }
        public Field Finish { get; }

        public MoveAction(Field start, Field finish)
        {
            Start= start;
            Finish = finish;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ICheckersAction);
        }

        public bool Equals(ICheckersAction? other)
        {
            if (other == null) return false;
            if (other is not MoveAction) return false;
            MoveAction moveAction = (MoveAction)other;
            if (!moveAction.Start.Equals(Start)) return false;
            if (!moveAction.Finish.Equals(Finish)) return false;
            return true;
        }

        public IEnumerable<Field> GetParticipatingFields()
        {
            yield return Start;
            yield return Finish;
        }

        public IEnumerable<Field> GetPlayableFields()
        {
            yield return Finish;
        }

        public CheckersState PerformOn(CheckersState state)
        {
            Piece movedPiece = state.GetPieceAt(Start);
            CheckersState newState = new(state);
            newState.SetPieceAt(Start, Piece.None);
            newState.SetPieceAtWithPossiblePromotion(Finish, movedPiece);
            newState.CurrentPlayer = state.CurrentPlayer == Player.One? Player.Two: Player.One;
            newState.LastAction = this;

            return newState;
        }
    }
}
