using Game.IGame;

namespace Game.Checkers
{
    public partial class Checkers : IGame<ICheckersAction, CheckersState, ICheckersInputState>
    {
        public GameResult Result(CheckersState state)
        {
            if (state.InsignificantActions == CheckersState.InsignificantActionsToDraw)
            {
                return GameResult.Draw;
            }
            if (!AnyAction(state))
            {
                if (state.CurrentPlayer == Player.One) return GameResult.PlayerTwoWins;
                return GameResult.PlayerOneWins;
            }
            return GameResult.InProgress;
        }

        private bool AnyAction(CheckersState state)
        {
            if (state.CurrentPlayer == Player.One)
            {
                for (int field = 0; field < CheckersState.FieldCount; field++)
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.WhitePawn && PossibleWhitePawnActions(state, field, 0).Item1.Any())
                        return true;
                    else if (piece == Piece.WhiteCrowned && PossibleCrownedActions(state, field, 0).Item1.Any())
                        return true;
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                for (int field = 0; field < CheckersState.FieldCount; field++)
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.BlackPawn && PossibleBlackPawnActions(state, field, 0).Item1.Any())
                        return true;
                    else if (piece == Piece.BlackCrowned && PossibleCrownedActions(state, field, 0).Item1.Any())
                        return true;
                }
            }
            return false;
        }
    }
}