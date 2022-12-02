using Game.IGame;
using Gdk;
using Gtk;
using LanguageExt;

namespace Game.Othello
{
    public class Othello : IGame<OthelloAction, OthelloState, LanguageExt.Unit>
    {
        public void DrawBoard(Widget widget, LanguageExt.Unit u, OthelloState state, IEnumerable<(OthelloAction, double)> ratedActions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OthelloAction> FilterByInputState(IEnumerable<OthelloAction> actions, LanguageExt.Unit u)
        {
            throw new NotImplementedException();
        }

        public GameResults GameResult(OthelloState state)
        {
            throw new NotImplementedException();
        }

        public (OthelloAction, LanguageExt.Unit) HandleInput(Event @event, LanguageExt.Unit u, OthelloState state)
        {
            throw new NotImplementedException();
        }

        public OthelloState PerformAction(OthelloAction action, OthelloState state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OthelloAction> PossibleActions(OthelloState state)
        {
            List<OthelloAction> actions = new List<OthelloAction>();



            return actions;
        }
    }
}