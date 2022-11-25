using Game.IGame;
using Gdk;
using Gtk;

namespace Game.Checkers
{
    public class Othello : IGame<OthelloAction, OthelloState, OthelloInputState>
    {
        public void DrawBoard(Widget widget, OthelloInputState inputState, OthelloState state, IEnumerable<(OthelloAction, double)> ratedActions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OthelloAction> FilterByInputState(IEnumerable<OthelloAction> actions, OthelloInputState inputState)
        {
            throw new NotImplementedException();
        }

        public GameResults GameResult(OthelloState state)
        {
            throw new NotImplementedException();
        }

        public (OthelloAction, OthelloInputState) HandleInput(Event @event, OthelloInputState inputState, OthelloState state)
        {
            throw new NotImplementedException();
        }

        public OthelloState PerformAction(OthelloAction action, OthelloState state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OthelloAction> PossibleActions(OthelloState state)
        {
            throw new NotImplementedException();
        }
    }
}