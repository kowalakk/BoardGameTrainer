using Cairo;
using Game.IGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BoardGameTrainer
{
    public interface IGameManager
    {
        public void Draw(Context context);
        public bool HandleInput(double x, double y);

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
            this.state = game.InitialState();
            this.inputState = game.EmptyInputState();   
        }

        public void Draw(Context context)
        {
            game.DrawBoard(context, inputState, state, Enumerable.Empty<(Action, double)>());
        }

        public bool HandleInput(double x, double y)
        {
            var (newInputState, action) = game.HandleInput(x, y, inputState, state);
            inputState = newInputState;
            if (action is not null)
            {
                state = game.PerformAction(action, state);
            }
            return game.Result(state) == GameResult.InProgress;
        }
    }
}