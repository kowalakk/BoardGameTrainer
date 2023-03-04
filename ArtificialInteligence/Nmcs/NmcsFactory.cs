using Game.IGame;

namespace Ai
{
    public class NmcsFactory : IAiFactory
    {
        public NmcsFactory() { }

        public IAi<Action, State, InputState> CreateAi<Action, State, InputState>(
            IGame<Action, State, InputState> game, 
            IStopCondition stopCondition)
        {
            throw new NotImplementedException();
        }
    }
}