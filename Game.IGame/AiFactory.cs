namespace Game.IGame
{
    public abstract class AiFactory<Action, State, InputState>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        protected abstract IAi<Action, State, InputState> MakeAi(
            IGame<Action, State, InputState> game);

        public IAi<Action, State, InputState> GetAi(
            IGame<Action, State, InputState> game)
        {
            return this.MakeAi(game);
        }
    }
}