namespace ArtificialInteligence
{
    public interface IArtificialIntelligence<State, Action, ModuleData>
    {
        List<(Action, double)> MoveAssessment<InputState>(IGame game, State state, ModuleData moduleData);
        Action ChooseMove(IGame game, State state);
    }
}