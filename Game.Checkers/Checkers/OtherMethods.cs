using Game.IGame;
using Gdk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<ICheckersAction, CheckersState, ICheckersInputState>
    {
        public Player CurrentPlayer(CheckersState state)
        {
            return state.CurrentPlayer;
        }

        public CheckersState InitialState()
        {
            return CheckersState.GetInitialState();
        }

        public ICheckersInputState EmptyInputState()
        {
            return new DefaultInputState();
        }

        //needs optimization
        public GameResult Result(CheckersState state)
        {
            IEnumerable<ICheckersAction> possibleActions = PossibleActions(state);
            if (possibleActions.Count() == 0)
            {
                if (state.CurrentPlayer == Player.One) return GameResult.PlayerTwoWins;
                return GameResult.PlayerOneWins;
            }
            return GameResult.InProgress;
        }

        public IEnumerable<(ICheckersAction, double)> FilterByInputState(
            IEnumerable<(ICheckersAction, double)> ratedActions, 
            ICheckersInputState inputState,
            int numberOfActions)
        {
            if (inputState is DefaultInputState)
                return ratedActions.OrderByDescending(tuple => tuple.Item2).Take(numberOfActions);
            if (inputState is MarkedPieceInputState cIS1)
                return ratedActions.Where(tuple => tuple.Item1.Start.Equals(cIS1.MarkedField));
            if (inputState is CaptureActionInProgressInputState cIS2)
                return ratedActions.Where(tuple =>
                {
                    IEnumerable<Field> fields = tuple.Item1.GetParticipatingFields();
                    return fields.Take(cIS2.VisitedFields.Count()).SequenceEqual(cIS2.VisitedFields);
                });
            throw new ArgumentException();
        }
    }
}