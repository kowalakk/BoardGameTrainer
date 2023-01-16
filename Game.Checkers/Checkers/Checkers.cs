using Game.IGame;
using Gdk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, ICheckersInputState>
    {
        public IEnumerable<(CheckersAction, double)> FilterByInputState(IEnumerable<(CheckersAction, double)> ratedActions, ICheckersInputState inputState)
        {
            if (inputState is IdleCIS)
                return ratedActions.OrderBy(tuple => tuple.Item2).Take(3);
            if (inputState is MarkedPieceCIS cIS1)
                return ratedActions.Where(tuple => tuple.Item1.Start.Equals(cIS1.MarkedField));
            if (inputState is CaptureActionsInProgressCIS cIS2)
                return (IEnumerable<(CheckersAction, double)>)cIS2.RatedCaptureActions;
            throw new ArgumentException();
        }

        public Player CurrentPlayer(CheckersState state)
        {
            return state.CurrentPlayer;
        }

        //needs optimization
        public GameResult Result(CheckersState state)
        {
            IEnumerable<CheckersAction> possibleActions = PossibleActions(state);
            if (possibleActions.Count() == 0)
            {
                if (state.CurrentPlayer == Player.One) return GameResult.PlayerTwoWins;
                return GameResult.PlayerOneWins;
            }
            return GameResult.InProgress;
        }

        public (ICheckersInputState, CheckersAction?) HandleInput(
            double x, 
            double y, 
            ICheckersInputState inputState, 
            CheckersState state,
            IEnumerable<(CheckersAction, double)> filteredActions)
        {
            if (x < 0 || x > 1 || y < 0 || x > 1)
            {
                return (new IdleCIS(), null);
            }
            if (inputState is IdleCIS)
            {
                int col = (int)(8 * x);
                int row = (int)(8 * y);
                Piece piece = state.GetPieceAt(col, row);

                if ((state.CurrentPlayer == Player.One && (piece == Piece.WhitePawn || piece == Piece.WhiteCrowned))
                    || (state.CurrentPlayer == Player.Two && (piece == Piece.BlackPawn || piece == Piece.BlackCrowned)))
                {
                    return (new MarkedPieceCIS(new Field(col, row)), null);
                }
                return (inputState, null);
            }
            if (inputState is MarkedPieceCIS cIS1)
            {

            }
            //    return ratedActions.Where(tuple => tuple.Item1.Start.Equals(cIS1.MarkedField));
            if (inputState is CaptureActionsInProgressCIS cIS2)
            {

            }
            //    return (IEnumerable<(CheckersAction, double)>)cIS2.RatedCaptureActions;
            throw new ArgumentException();
        }

    }
}