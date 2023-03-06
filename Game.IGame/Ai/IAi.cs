namespace Game.IGame
{
    public interface IAi<Action, State, InputState>
    {
        List<(Action, double)> MoveAssessment(State state);
        Action ChooseAction(State state);
    }
}