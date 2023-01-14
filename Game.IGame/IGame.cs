using Cairo;
using Gdk;
using Gtk;

namespace Game.IGame
{
    public enum GameResult
    {
        InProgress = 0,
        PlayerOneWins = 1,
        PlayerTwoWins = 2,
        Draw = 3,
    }
    public enum Player
    {
        PlayerOne = GameResult.PlayerOneWins,
        PlayerTwo = GameResult.PlayerTwoWins,
    }

    public interface IGame { }
    public interface IGame<Action, State, InputState> : IGame
        where Action: IEquatable<Action>
        where State: IEquatable<State> 
    {
        public State PerformAction(Action action, State state);

        public IEnumerable<Action> PossibleActions(State state);

        public GameResult Result(State state);

        public Player CurrentPlayer(State state);

        public void DrawBoard(Context context, InputState inputState, State state, IEnumerable<(Action, double)> ratedActions);

        public (Action, InputState) HandleInput(Event @event, InputState inputState, State state);

        public IEnumerable<Action> FilterByInputState(IEnumerable<Action> actions, InputState inputState);
    }

}