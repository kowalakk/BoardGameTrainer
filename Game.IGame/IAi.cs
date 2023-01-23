namespace Game.IGame
{
    public interface IAi<Action, State, InputState>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        List<(Action, double)> MoveAssessment(State state);
        Action ChooseMove(State state);
    }
}