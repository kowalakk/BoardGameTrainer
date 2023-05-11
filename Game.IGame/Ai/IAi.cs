namespace Game.IGame
{
    public interface IAi<Action, State, InputState>
    {
        List<(Action, double)> MoveAssessment(State state, CancellationToken token);
        Action ChooseAction(State state, CancellationToken token);
    }
}