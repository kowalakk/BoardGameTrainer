using Game.IGame;

namespace Ai
{
    internal class NMCS<Action, State, InputState> : IAi<Action, State, InputState>
    {
        public List<(Action, double)> MoveAssessment(GameTree<Action, State> gameTree)
        {
            throw new NotImplementedException();
        }

        public Action ChooseAction(GameTree<Action, State> gameTree)
        {
            throw new NotImplementedException();
        }

        void NMCSearch(Node<Action, State> root, int level, int timeInterval)
        {
            throw new NotImplementedException();
        }
        List<Action> Nested(Node<Action, State> node, int level)
        {
            throw new NotImplementedException();
        }

        public bool MoveGameToNextState(GameTree<Action, State> gameTree, Action action)
        {
            throw new NotImplementedException();
        }

        public bool MoveGameToPreviousState(GameTree<Action, State> gameTree, Action action)
        {
            throw new NotImplementedException();
        }
    }
}
