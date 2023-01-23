namespace Game.IGame

{
    public abstract class AiFactory<Action, State, InputState>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        protected abstract IAi<Action, State, InputState> MakeUCT(IGame<Action, State, InputState> game);

        protected abstract IAi<Action, State, InputState> MakeNMCS(IGame<Action, State, InputState> game);

        public IAi<Action, State, InputState> GetUCT(IGame<Action, State, InputState> game)
        {
            return this.MakeUCT(game);
        }

        public IAi<Action, State, InputState> GetNMCS(IGame<Action, State, InputState> game)
        {
            return this.MakeNMCS(game);
        }
    }
}