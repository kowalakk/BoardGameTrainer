using Cairo;

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

        public void DrawBoard(Context context, int numberOfShownHints)
        {
            game.DrawBoard(context, inputState, state, game.FilterByInputState(ratedActions, inputState, numberOfShownHints));
        }

        public GameResult HandleInput(double x, double y, bool isPlayer2Ai)
        {
            var (newInputState, action) = game.HandleInput(x, y, inputState, state);
            inputState = newInputState;
            GameResult gameResult = GameResult.InProgress;
            if (action is not null)
            {
                state = game.PerformAction(action, state);
                gameResult = game.Result(state);
                if (gameResult == GameResult.InProgress)
                {
                    if (isPlayer2Ai)
                    {
                        state = game.PerformAction(ai.ChooseAction(state), state);
                        gameResult = game.Result(state);
                    }
                    ratedActions = ai.MoveAssessment(state);
                }
            }
            return gameResult;
        }
    }
}