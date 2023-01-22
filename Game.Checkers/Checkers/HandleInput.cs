﻿using Game.IGame;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, ICheckersInputState>
    {

        public (ICheckersInputState, CheckersAction?) HandleInput(
            double x,
            double y,
            ICheckersInputState inputState,
            CheckersState state)
        {
            if (x < 0 || x > 1 || y < 0 || x > 1)
            {
                return (new DefaultCIS(), null);
            }
            int col = (int)(8 * x);
            int row = CheckersState.BOARD_SIZE - 1 - (int)(8 * y);
            Piece piece = state.GetPieceAt(col, row);
            Field clickedField = new Field(col, row);
            if (inputState is DefaultCIS)
            {
                return HandleFirstClick(piece, clickedField, inputState, state);
            }
            if (inputState is MarkedPieceCIS markedPieceCIS)
            {
                CheckersAction? nextAction = PossibleActions(state)
                    .Where(action => action.Start.Equals(markedPieceCIS.MarkedField))
                    .FirstOrDefault(action => action.GetPlayableFields().Contains(clickedField));
                if (nextAction == default) // unrelated field got chosen
                {
                    return HandleFirstClick(piece, clickedField, inputState, state);
                }
                if (nextAction.Finish.Equals(clickedField)) // whole action chosen
                    return (new DefaultCIS(), nextAction);
                // part of action chosen
                IEnumerable<Field> visitedFields = nextAction.GetParticipatingFields()
                    .TakeWhile(field => !field.Equals(clickedField));
                return (new CaptureActionInProgressCIS(visitedFields.Append(clickedField)), null);
            }
            {
                CaptureActionInProgressCIS cIS = (CaptureActionInProgressCIS)inputState;
                if (cIS.VisitedFields.Contains(clickedField)) // used field got chosen 
                    return (cIS, null);
                IEnumerable<CheckersAction> possibleActionsInProgress = PossibleActions(state).Where(
                    action => action.GetParticipatingFields()
                    .Take(cIS.VisitedFields.Count()).SequenceEqual(cIS.VisitedFields));
                CheckersAction? nextAction = possibleActionsInProgress.FirstOrDefault(action => action.GetPlayableFields().Contains(clickedField));
                if (nextAction == default) // unrelated field got chosen
                {
                    return (cIS, null);
                }
                if (nextAction.Finish.Equals(clickedField)) // whole action chosen
                    return (new DefaultCIS(), nextAction);
                // part of action chosen
                IEnumerable<Field> visitedFields = nextAction.GetParticipatingFields().TakeWhile(field => !field.Equals(clickedField));
                return (new CaptureActionInProgressCIS(visitedFields.Append(clickedField)), null);
            }

        }

        private (ICheckersInputState, CheckersAction?) HandleFirstClick(Piece piece, Field clickedField, ICheckersInputState inputState, CheckersState state)
        {
            if ((state.CurrentPlayer == Player.One && (piece == Piece.WhitePawn || piece == Piece.WhiteCrowned))
    || (state.CurrentPlayer == Player.Two && (piece == Piece.BlackPawn || piece == Piece.BlackCrowned)))
            {
                return (new MarkedPieceCIS(clickedField), null);
            }
            return (new DefaultCIS(), null);
        }
    }
}
