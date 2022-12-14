using ArtificialIntelligence;
using Game.IGame;

namespace ArtificialInteligence
{
    public interface IArtificialIntelligence<Action, State, InputState, ModuleData>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        List<(Action, double)> MoveAssessment(IGame<Action, State, InputState> game, State state, ModuleData moduleData, IStopCondition condition);
        Action ChooseMove(IGame<Action, State, InputState> game, State state);
    }
}