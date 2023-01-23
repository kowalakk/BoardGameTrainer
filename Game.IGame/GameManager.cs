using Cairo;

namespace Game.IGame
{
    public interface IGameManager
    {
        public void DrawBoard(Context context);
        public GameResult HandleInput(double x, double y);
    }

    public class GameManager<Action, State, InputState> : IGameManager
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        public readonly IGame<Action, State, InputState> game;
        private State state;
        private InputState inputState;

        public GameManager(IGame<Action, State, InputState> game)
        {
            this.game = game;
            state = game.InitialState();
            inputState = game.EmptyInputState();
        }

        public void DrawBoard(Context context)
        {
            game.DrawBoard(context, inputState, state, new List<(Action, double)>());
        }

        public GameResult HandleInput(double x, double y)
        {
            var (newInputState, action) = game.HandleInput(x, y, inputState, state);
            inputState = newInputState;
            if (action is not null)
            {
                state = game.PerformAction(action, state);
            }
            return game.Result(state);
        }
    }
}