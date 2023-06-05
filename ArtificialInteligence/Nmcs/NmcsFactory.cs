using Game.IGame;

namespace Ai
{
    public class NmcsFactory : IAiFactory
    {
        private int Depth { get; }

        public NmcsFactory(int depth)
        {
            Depth = depth;
        }

        public IAi<Action, State, InputState> CreateAi<Action, State, InputState>(
            IGame<Action, State, InputState> game,
            IStopCondition stopCondition)
        {
            return new Nmcs<Action, State, InputState>(Depth, game, stopCondition);
        }
    }
}