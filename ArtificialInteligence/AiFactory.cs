using Game.IGame;

namespace Ai
{
    public class AiFactory<Action, State, InputState>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        public IAi<Action, State, InputState> GetUCT(IGame<Action, State, InputState> game)
        {
            return new UCT<Action, State, InputState>(1.41, game, new IterationStopCondition(1000));
        }

        public IAi<Action, State, InputState> GetNMCS(IGame<Action, State, InputState> game)
        {
            throw new NotImplementedException();
        }
    }
}