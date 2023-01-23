namespace AI
{
    internal class NMCS<Action, State, InputState> : IAi<Action, State, InputState, NMCSModuleData<Action, State>>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        public Action ChooseMove( State state)
        {
            throw new NotImplementedException();
        }

        public List<(Action, double)> MoveAssessment(State state)
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
