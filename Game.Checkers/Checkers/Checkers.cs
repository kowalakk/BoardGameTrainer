﻿using Game.IGame;
using Gdk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, CheckersInputState>
    {
        public IEnumerable<CheckersAction> FilterByInputState(IEnumerable<CheckersAction> actions, CheckersInputState inputState)
        {
            throw new NotImplementedException();
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
                if (state.CurrentPlayer == Player.PlayerOne) return GameResult.PlayerTwoWins;
                return GameResult.PlayerOneWins;
            }
            return GameResult.InProgress;
        }

        public (CheckersAction, CheckersInputState) HandleInput(Event @event, CheckersInputState inputState, CheckersState state)
        {
            throw new NotImplementedException();
        }

    }
}