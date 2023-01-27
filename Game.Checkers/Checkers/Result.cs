using Game.IGame;
using Gdk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<ICheckersAction, CheckersState, ICheckersInputState>
    {
        //needs optimization
        public GameResult Result(CheckersState state)
        {
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
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.WhitePawn && PossiblePawnActions(state, field, 0, 1).Item1.Any())
                    {
                        return true;
                    }
                    else if (piece == Piece.WhiteCrowned && PossibleCrownedActions(state, field, 0).Item1.Any())
                    {
                        return true;
                    }
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.BlackPawn && PossiblePawnActions(state, field, 0, -1).Item1.Any())
                        return true;
                    else if (piece == Piece.BlackCrowned && PossibleCrownedActions(state, field, 0).Item1.Any())
                        return true;

                }
            }
            return false;
        }
    }
}