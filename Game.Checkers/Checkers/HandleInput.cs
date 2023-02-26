using Game.IGame;

namespace Game.Checkers
{
    public partial class Checkers : IGame<ICheckersAction, CheckersState, ICheckersInputState>
    {

        public (ICheckersInputState, ICheckersAction?) HandleInput(
            double x,
            double y,
            ICheckersInputState inputState,
            CheckersState state)
        {
            if (x < 0 || x > 1 || y < 0 || y > 1)
            {
                return (new DefaultInputState(), null);
            }
            int col = (int)(8 * x);
            int row = (int)(8 * y);
            int clickedField = -1;
            Piece piece = Piece.None;
            if (col % 2 != row % 2)
            {
                clickedField = col / 2 + 4 * row;
                piece = state.GetPieceAt(clickedField);
            }
            if (inputState is DefaultInputState)
            {
                return HandleFirstClick(piece, clickedField, state);
            }
            if (inputState is MarkedPieceInputState mpInputState)
            {
                ICheckersAction? nextAction = PossibleActions(state)
                    .Where(action => action.Start.Equals(mpInputState.MarkedField))
                    .FirstOrDefault(action => action.GetPlayableFields().Contains(clickedField));
                if (nextAction == default) // unrelated field got chosen
                {
                    return HandleFirstClick(piece, clickedField, state);
                }
                if (nextAction.Finish.Equals(clickedField)) // whole action chosen
                    return (new DefaultInputState(), nextAction);
                // part of action chosen
                IEnumerable<int> visitedFields = nextAction.GetParticipatingFields()
                    .TakeWhile(field => !field.Equals(clickedField));
                return (new CaptureActionInProgressInputState(visitedFields.Append(clickedField)), null);
            }
            {
                CaptureActionInProgressInputState caipInputState = (CaptureActionInProgressInputState)inputState;
                if (caipInputState.VisitedFields.Contains(clickedField)) // used field got chosen 
                    return (caipInputState, null);
                IEnumerable<ICheckersAction> possibleActionsInProgress = PossibleActions(state).Where(
                    action => action.GetParticipatingFields()
                    .Take(caipInputState.VisitedFields.Count()).SequenceEqual(caipInputState.VisitedFields));
                ICheckersAction? nextAction = possibleActionsInProgress.FirstOrDefault(action => action.GetPlayableFields().Contains(clickedField));
                if (nextAction == default) // unrelated field got chosen
                {
                    return (caipInputState, null);
                }
                if (nextAction.Finish.Equals(clickedField)) // whole action chosen
                    return (new DefaultInputState(), nextAction);
                // part of action chosen
                IEnumerable<int> visitedFields = nextAction.GetParticipatingFields().TakeWhile(field => !field.Equals(clickedField));
                return (new CaptureActionInProgressInputState(visitedFields.Append(clickedField)), null);
            }

        }

        private (ICheckersInputState, ICheckersAction?) HandleFirstClick(
            Piece piece,
            int clickedField,
            CheckersState state)
        {
            if ((state.CurrentPlayer == Player.One && (piece == Piece.WhitePawn || piece == Piece.WhiteCrowned))
                || (state.CurrentPlayer == Player.Two && (piece == Piece.BlackPawn || piece == Piece.BlackCrowned)))
            {
                return (new MarkedPieceInputState(clickedField), null);
            }
            return (new DefaultInputState(), null);
        }
    }
}
