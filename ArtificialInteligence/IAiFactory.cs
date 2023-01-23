using Game.IGame;

namespace Ai
{
    public interface IAiFactory
    {
        public IAi<Action, State, InputState> CreateAi<Action, State, InputState>(IGame<Action, State, InputState> game)
    where Action : IEquatable<Action>
    where State : IEquatable<State>;
    }
}