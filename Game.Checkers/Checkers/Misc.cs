using Game.IGame;
using Gdk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, ICheckersInputState>
    {
        public IEnumerable<(CheckersAction, double)> FilterByInputState(IEnumerable<(CheckersAction, double)> ratedActions, ICheckersInputState inputState)
        {
            if (inputState is IdleCIS)
                return ratedActions.OrderByDescending(tuple => tuple.Item2).Take(3);
            if (inputState is MarkedPieceCIS cIS1)
                return ratedActions.Where(tuple => tuple.Item1.Start.Equals(cIS1.MarkedField));
            if (inputState is CaptureActionInProgressCIS cIS2)
                return ratedActions.Where(tuple =>
                {
                    IEnumerable<Field> fields = tuple.Item1.GetClickableFields();
                    return fields.Take(cIS2.VisitedFields.Count()).SequenceEqual(cIS2.VisitedFields);
                });
            throw new ArgumentException();
        }

        public Player CurrentPlayer(CheckersState state)
        {
            return state.CurrentPlayer;
        }

        //needs optimization
        public GameResult Result(CheckersState state)
        {
            IEnumerable<CheckersAction> possibleActions = PossibleActions(state);
            if (possibleActions.Count() == 0)
            {
                if (state.CurrentPlayer == Player.One) return GameResult.PlayerTwoWins;
                return GameResult.PlayerOneWins;
            }
            return GameResult.InProgress;
        }
    }
}