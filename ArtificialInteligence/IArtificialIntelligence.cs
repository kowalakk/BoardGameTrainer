using ArtificialIntelligence;
using Game.IGame;

namespace ArtificialInteligence
{
    public interface IArtificialIntelligence<Action, State, InputState, ModuleData>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        List<(Action, double)> MoveAssessment(State state);
        Action ChooseMove(State state);
    }
}