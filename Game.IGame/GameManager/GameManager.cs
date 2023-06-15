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
            CancellationTokenSource tokenSource = new();
            ratedActions = ai.MoveAssessment(gameTree, tokenSource.Token);
        }

        public void DrawBoard(Context context, int numberOfHints)
        {
            GameResult gameResult = game.Result(currentState);
            if (gameResult == GameResult.InProgress)
            {
                IEnumerable<(Action, double)> filteredActions =
                    game.FilterByInputState(ratedActions, currentInputState, numberOfHints);
                game.DrawBoard(context, currentInputState, currentState, filteredActions);
                return;
            }

            game.DrawBoard(context, currentInputState, currentState, Enumerable.Empty<(Action, double)>());
            string text = string.Empty;
            if (gameResult == GameResult.PlayerOneWins)
            {
                text = "Player One Wins!";
                context.MoveTo(0, 0.5);
            }
            else if (gameResult == GameResult.PlayerTwoWins)
            {
                text = "Player Two Wins!";
                context.MoveTo(0, 0.5);
            }
            else if (gameResult == GameResult.Draw)
            {
                text = "A Draw!";
                context.MoveTo(0.25, 0.5);
            }
            context.SetSourceRGBA(0, 0, 1, 0.7);
            context.SelectFontFace("Sans", FontSlant.Italic, FontWeight.Bold);
            context.SetFontSize(0.12);
            context.ShowText(text);
            context.Stroke();
        }

        public (GameResult result, bool actionPerformed) HandleMovement(double x, double y)
        {
            (currentInputState, Action? nextAction) = game.HandleInput(x, y, currentInputState, currentState);
            if (nextAction is not null)
            {
                ai.MoveGameToNextState(gameTree, nextAction);
                currentState = gameTree.SelectedNode.CorespondingState;
                GameResult gameResult = game.Result(currentState);
                ratedActions = new List<(Action, double)>();
                return (gameResult, true);
            }
            return (game.Result(currentState), false);
        }

        public GameResult HandleAiMovement()
        {
            CancellationToken token = new();
            Action nextAction = ai.ChooseAction(gameTree, token);
            if (nextAction is not null)
            {
                ai.MoveGameToNextState(gameTree, nextAction);
                currentState = gameTree.SelectedNode.CorespondingState;
                GameResult gameResult = game.Result(currentState);
                ratedActions = new List<(Action, double)>();
                return gameResult;
            }
            return game.Result(currentState);
        }

        public void ComputeHints(CancellationToken token)
        {
            ratedActions = ai.MoveAssessment(gameTree, token);
        }

        public void Restart()
        {
            currentState = game.InitialState();
            currentInputState = game.EmptyInputState();
            gameTree = new GameTree<Action, State>(currentState);
            CancellationTokenSource tokenSource = new();
            ratedActions = ai.MoveAssessment(gameTree, tokenSource.Token);
        }

        public Player CurrentPlayer()
        {
            return game.CurrentPlayer(currentState);
        }
    }
}