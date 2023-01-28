using Game.IGame;

namespace Game.Checkers
{
    public class MoveAction : ICheckersAction
    {
        public int Start { get; }
        public int Finish { get; }

        public MoveAction(int start, int finish)
        {
            Start= start;
            Finish = finish;
        }

        public IEnumerable<int> GetParticipatingFields()
        {
            yield return Start;
            yield return Finish;
        }

        public IEnumerable<int> GetPlayableFields()
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

        public override bool Equals(object? other)
        {
            if (other == null) return false;
            if (other is not MoveAction) return false;
            MoveAction moveAction = (MoveAction)other;
            if (!moveAction.Start.Equals(Start)) return false;
            if (!moveAction.Finish.Equals(Finish)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ Finish.GetHashCode();
        }
    }
}
