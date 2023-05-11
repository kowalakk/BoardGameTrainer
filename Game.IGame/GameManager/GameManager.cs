using Cairo;
using System.Threading;

namespace Game.IGame
{
    public class GameManager<Action, State, InputState> : IGameManager
    {
        private readonly IGame<Action, State, InputState> game;
        private readonly IAi<Action, State, InputState> ai;
        private State currentState;
        private InputState currentInputState;
        private GameTree<Action, State> gameTree;
        private List<(Action, double)> ratedActions;

        public GameManager(IGame<Action, State, InputState> game, IAiFactory aiFactory, IStopCondition stopCondition)
        {
            this.game = game;
            ai = aiFactory.CreateAi(game, stopCondition);
            currentState = game.InitialState();
            currentInputState = game.EmptyInputState();
            gameTree = new GameTree<Action, State>(currentState);
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ratedActions = ai.MoveAssessment(gameTree, tokenSource.Token);
        }

        public void DrawBoard(Context context, (int, int) numberOfHints)
        {
            int numberOfActions = game.CurrentPlayer(currentState) == Player.One ? numberOfHints.Item1 : numberOfHints.Item2;
            IEnumerable<(Action, double)> filteredActions = game.FilterByInputState(ratedActions, currentInputState, numberOfActions);
            game.DrawBoard(context, currentInputState, currentState, filteredActions);
        }

        public GameResult HandleMovement(double x, double y, bool isPlayer2Ai)
        {
            (currentInputState, Action? nextAction) = game.HandleInput(x, y, currentInputState, currentState);
            GameResult gameResult = GameResult.InProgress;
            if (nextAction is not null)
            {
                ai.MoveGameToNextState(gameTree, nextAction);
                currentState = gameTree.SelectedNode.CorespondingState;
                gameResult = game.Result(currentState);
                if (gameResult == GameResult.InProgress)
                {
                    if (isPlayer2Ai)
                    {
                        CancellationToken token = new CancellationToken();
                        nextAction = ai.ChooseAction(gameTree, token);
                        ai.MoveGameToNextState(gameTree, nextAction);
                        currentState = gameTree.SelectedNode.CorespondingState;
                        gameResult = game.Result(currentState);
                    }
                }
            }
            return gameResult;
        }

        public void ComputeHints(CancellationToken token)
        {
            ratedActions = ai.MoveAssessment(gameTree, token);
        }
    }
}