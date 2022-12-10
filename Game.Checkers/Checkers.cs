﻿using Game.IGame;
using Gdk;
using Gtk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, CheckersInputState>
    {
        public void DrawBoard(Widget widget, CheckersInputState inputState, CheckersState state, IEnumerable<(CheckersAction, double)> ratedActions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CheckersAction> FilterByInputState(IEnumerable<CheckersAction> actions, CheckersInputState inputState)
        {
            throw new NotImplementedException();
        }

        //needs optimization
        public GameResults GameResult(CheckersState state)
        {
            IEnumerable<CheckersAction> possibleActions = PossibleActions(state);
            if (possibleActions.Count() == 0)
            {
                if (state.CurrentPlayer == Player.White) return GameResults.PlayerTwoWins;
                return GameResults.PlayerOneWins;
            }
            return GameResults.InProgress;
        }

        public (CheckersAction, CheckersInputState) HandleInput(Event @event, CheckersInputState inputState, CheckersState state)
        {
            throw new NotImplementedException();
        }

    }
}