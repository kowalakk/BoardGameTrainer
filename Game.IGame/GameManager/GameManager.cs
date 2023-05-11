using Cairo;

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
            ratedActions = ai.MoveAssessment(gameTree);
        }

        public void DrawBoard(Context context, (int, int) numberOfHints)
        {
            int numberOfActions = game.CurrentPlayer(currentState) == Player.One ? numberOfHints.Item1 : numberOfHints.Item2;
            IEnumerable<(Action, double)> filteredActions = game.FilterByInputState(ratedActions, currentInputState, numberOfActions);
            game.DrawBoard(context, currentInputState, currentState, filteredActions);
        }

        public GameResult HandleInput(double x, double y, bool isPlayer2Ai)
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
                        nextAction = ai.ChooseAction(gameTree);
                        ai.MoveGameToNextState(gameTree, nextAction);
                        currentState = gameTree.SelectedNode.CorespondingState;
                        gameResult = game.Result(currentState);
                    }
                    ratedActions = ai.MoveAssessment(gameTree);
                }
            }
            return gameResult;
        }
    }
}