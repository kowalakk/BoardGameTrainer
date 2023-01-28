using Game.IGame;
using Gdk;
using Gtk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<ICheckersAction, CheckersState, ICheckersInputState>
    {
        public CheckersState PerformAction(ICheckersAction action, CheckersState state)
        {
            return action.PerformOn(state);
        }

        private static CheckersState PerformTemporaryCapture(CaptureAction action, CheckersState state)
        {
            return action.PerformOn(state, Piece.CapturedPiece);
        }

    }
}