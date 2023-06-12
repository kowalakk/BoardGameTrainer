using Game.IGame;

namespace Ai
{
    public abstract class Ai<Action, State, InputState> : IAi<Action, State, InputState>
    {
        protected IGame<Action, State, InputState> Game { get; }
        protected IStopCondition StopCondition { get; set; }

        public Ai(IGame<Action, State, InputState> game, IStopCondition stopCondition)
        {
            Game = game;
            StopCondition = stopCondition;
        }
        public Action ChooseAction(GameTree<Action, State> gameTree, CancellationToken token)
        {
            return MoveAssessment(gameTree, token)
                .MaxBy(action => { return action.Item2; })
                .Item1;
        }

        public abstract List<(Action, double)> MoveAssessment(GameTree<Action, State> gameTree, CancellationToken token);

        protected GameResult DefaultPolicy(State state)
        {
            GameResult gameResult = Game.Result(state);
            while (gameResult == GameResult.InProgress)
            {
                IEnumerable<Action> possibleActions = Game.PossibleActions(state);
                Action randomAction = possibleActions.RandomElement();
                state = Game.PerformAction(randomAction, state);
                gameResult = Game.Result(state);
            }
            return gameResult;
        }

        protected int Delta(State state, GameResult gameResult)
        {
            if (gameResult == GameResult.Draw)
                return 0;
            if (gameResult == (GameResult)Game.CurrentPlayer(state))
                return 1;
            return -1;
        }
        public void MoveGameToNextState(GameTree<Action, State> gameTree, Action action)
        {
            gameTree.SelectChildNode(action);
        }

        public void MoveGameToPreviousState(GameTree<Action, State> gameTree, Action action)
        {
            gameTree.SelectParentNode();
        }
    }
}
