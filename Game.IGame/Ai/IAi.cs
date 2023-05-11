using System.Threading;

namespace Game.IGame
{
    public interface IAi<Action, State, InputState>
    {
        List<(Action, double)> MoveAssessment(GameTree<Action, State> gameTree, CancellationToken token);

        Action ChooseAction(GameTree<Action, State> gameTree, CancellationToken token);

        void MoveGameToNextState(GameTree<Action, State> gameTree, Action action);

        void MoveGameToPreviousState(GameTree<Action, State> gameTree, Action action);
    }
}