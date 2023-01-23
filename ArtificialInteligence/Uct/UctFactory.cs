using Game.IGame;

namespace Ai
{
    public class UctFactory : IAiFactory
    {
        private double UctConstant { get; }

        public UctFactory(double uctConstant)
        {
            UctConstant = uctConstant;
        }

        public IAi<Action, State, InputState> CreateAi<Action, State, InputState>(IGame<Action, State, InputState> game)
    where Action : IEquatable<Action>
    where State : IEquatable<State>
        {
            return new Uct<Action, State, InputState>(UctConstant, game, new IterationStopCondition(1000));
        }
    }
}