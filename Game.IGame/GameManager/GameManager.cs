using Cairo;

namespace Game.IGame
{
    public class GameManager<Action, State, InputState> : IGameManager
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
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ratedActions = ai.MoveAssessment(state, tokenSource.Token);
        }

        public void DrawBoard(Context context, (int, int) numberOfHints)
        {
            int numberOfActions = game.CurrentPlayer(state) == Player.One? numberOfHints.Item1 :  numberOfHints.Item2;
            IEnumerable<(Action, double)> filteredActions = game.FilterByInputState(ratedActions, inputState, numberOfActions);
            game.DrawBoard(context, inputState, state, filteredActions);
        }

        public GameResult HandleMovement(double x, double y, bool isPlayer2Ai)
        {
            var (newInputState, action) = game.HandleInput(x, y, inputState, state);
            inputState = newInputState;
            GameResult gameResult = GameResult.InProgress;
            if (action is not null)
            {
                state = game.PerformAction(action, state);
                gameResult = game.Result(state);
                ratedActions = new List<(Action, double)>();
                if (gameResult == GameResult.InProgress)
                {
                    if (isPlayer2Ai)
                    {
                        CancellationToken token = new CancellationToken();
                        state = game.PerformAction(ai.ChooseAction(state, token), state);
                        gameResult = game.Result(state);
                    }
                }
            }
            return gameResult;
        }

        public void ComputeHints(CancellationToken token)
        {
            ratedActions = ai.MoveAssessment(state, token);
        }
    }
}