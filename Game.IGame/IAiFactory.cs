namespace Game.IGame
{
    public interface IAiFactory
    {
        public IAi<Action, State, InputState> CreateAi<Action, State, InputState>(
            IGame<Action, State, InputState> game, 
            IStopCondition stopCondition)
    where Action : IEquatable<Action>
    where State : IEquatable<State>;
    }
}