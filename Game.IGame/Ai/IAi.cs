namespace Game.IGame
{
    public interface IAi<Action, State, InputState>
    {
        List<(Action, double)> MoveAssessment(GameTree<Action, State> gameTree);

        Action ChooseAction(GameTree<Action, State> gameTree);

        bool MoveGameToNextState(GameTree<Action, State> gameTree, Action action);

        bool MoveGameToPreviousState(GameTree<Action, State> gameTree, Action action);

        void ResetGame(GameTree<Action, State> gameTree);
    }
}