using Cairo;

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
        One = GameResult.PlayerOneWins,
        Two = GameResult.PlayerTwoWins,
    }

    public interface IGame { }
    public interface IGame<Action, State, InputState> : IGame
    {
        public State InitialState();

        public InputState EmptyInputState();
        
        public Player CurrentPlayer(State state);
        
        public IEnumerable<Action> PossibleActions(State state);

        public State PerformAction(Action action, State state);

        public GameResult Result(State state);

        public void DrawBoard(Context context, InputState inputState, State state, IEnumerable<(Action, double)> ratedActions);

        public (InputState, Action?) HandleInput(double x, double y, InputState inputState, State state);

        public IEnumerable<(Action, double)> FilterByInputState(
            IEnumerable<(Action, double)> ratedActions, 
            InputState inputState,
            int numberOfActions);
    }

}