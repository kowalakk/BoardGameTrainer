using Game.IGame;
using Gdk;
using Gtk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, ICheckersInputState>
    {
        public CheckersState PerformAction(CheckersAction action, CheckersState state)
        {
            return action.PerformOn(state);
        }

        private static CheckersState PerformTemporaryCapture(CaptureAction action, CheckersState state)
        {
            return action.PerformOn(state, Piece.CapturedPiece);
        }

    }
}