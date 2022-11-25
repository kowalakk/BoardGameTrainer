using Game.IGame;
using Gdk;
using Gtk;

namespace Game.Checkers
{
    public class Checkers : IGame<CheckersAction, CheckersState, CheckersInputState>
    {
        public void DrawBoard(Widget widget, CheckersInputState inputState, CheckersState state, IEnumerable<(CheckersAction, double)> ratedActions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CheckersAction> FilterByInputState(IEnumerable<CheckersAction> actions, CheckersInputState inputState)
        {
            throw new NotImplementedException();
        }

        public GameResults GameResult(CheckersState state)
        {
            throw new NotImplementedException();
        }

        public (CheckersAction, CheckersInputState) HandleInput(Event @event, CheckersInputState inputState, CheckersState state)
        {
            throw new NotImplementedException();
        }

        public CheckersState PerformAction(CheckersAction action, CheckersState state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CheckersAction> PossibleActions(CheckersState state)
        {
            throw new NotImplementedException();
        }
    }
}