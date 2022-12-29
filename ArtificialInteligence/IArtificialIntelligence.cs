namespace AI
{
    public interface IArtificialIntelligence<Action, State, InputState, ModuleData>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        List<(Action, double)> MoveAssessment(State state);
        Action ChooseMove(State state);
    }
}