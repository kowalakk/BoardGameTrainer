namespace Game.IGame
{
    public interface IAi<Action, State, InputState>
    {
        List<(Action, double)> MoveAssessment(GameTree<Action, State> gameTree);

        Action ChooseAction(GameTree<Action, State> gameTree);

        void MoveGameToNextState(GameTree<Action, State> gameTree, Action action);

        void MoveGameToPreviousState(GameTree<Action, State> gameTree, Action action);
    }
}