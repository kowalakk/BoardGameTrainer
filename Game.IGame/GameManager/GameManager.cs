﻿using Cairo;

namespace Game.IGame
{
    public class GameManager<Action, State, InputState> : IGameManager
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        public readonly IGame<Action, State, InputState> game;
        private State state;
        private InputState inputState;
        private IAi<Action, State, InputState> ai;
        private List<(Action, double)> ratedActions;

        public GameManager(IGame<Action, State, InputState> game, IAiFactory aiFactory, IStopCondition stopCondition)
        {
            this.game = game;
            state = game.InitialState();
            inputState = game.EmptyInputState();
            ai = aiFactory.CreateAi(game, stopCondition);
            ratedActions = ai.MoveAssessment(state);
        }

        public void DrawBoard(Context context, int bestShownActionsCount)
        {
            game.DrawBoard(context, inputState, state, game.FilterByInputState(ratedActions, inputState, bestShownActionsCount));
        }

        public GameResult HandleInput(double x, double y)
        {
            var (newInputState, action) = game.HandleInput(x, y, inputState, state);
            inputState = newInputState;
            if (action is not null)
            {
                state = game.PerformAction(action, state);
                ratedActions = ai.MoveAssessment(state);
            }
            return game.Result(state);
        }
    }
}