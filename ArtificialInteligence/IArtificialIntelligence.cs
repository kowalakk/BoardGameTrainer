using Game.IGame;

namespace ArtificialInteligence
{
    public interface IArtificialIntelligence<Action, State, ModuleData>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        List<(Action, double)> MoveAssessment<InputState>(IGame<Action, State, InputState> game, State state, ModuleData moduleData);
        Action ChooseMove<InputState>(IGame<Action, State, InputState> game, State state);
    }
}