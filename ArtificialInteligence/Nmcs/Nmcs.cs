using Game.IGame;

namespace Ai
{
    internal class NMCS<Action, State, InputState> : IAi<Action, State, InputState>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        public Action ChooseAction( State state, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public List<(Action, double)> MoveAssessment(State state, CancellationToken token)
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
    }
}
