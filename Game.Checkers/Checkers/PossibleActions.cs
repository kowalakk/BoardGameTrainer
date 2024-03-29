﻿using Game.IGame;

namespace Game.Checkers
{
    public partial class Checkers : IGame<ICheckersAction, CheckersState, ICheckersInputState>
    {
        public IEnumerable<ICheckersAction> PossibleActions(CheckersState state)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleActions = (new(), 0);
            if (state.CurrentPlayer == Player.One)
            {
                for (int field = 0; field < CheckersState.FieldCount; field++)
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.WhitePawn)
                        possibleActions = UpdateActions(
                            possibleActions,
                            PossibleWhitePawnActions(state, field, possibleActions.maxCapturesCount));
                    else if (piece == Piece.WhiteCrowned)
                        possibleActions = UpdateActions(
                            possibleActions,
                            PossibleCrownedActions(state, field, possibleActions.maxCapturesCount));
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                for (int field = 0; field < CheckersState.FieldCount; field++)
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.BlackPawn)
                        possibleActions = UpdateActions(
                            possibleActions,
                            PossibleBlackPawnActions(state, field, possibleActions.maxCapturesCount));
                    else if (piece == Piece.BlackCrowned)
                        possibleActions = UpdateActions(
                            possibleActions,
                            PossibleCrownedActions(state, field, possibleActions.maxCapturesCount));
                }
            }
            return possibleActions.list;
        }

        private (List<ICheckersAction>, int) PossibleWhitePawnActions(
            CheckersState state, int field, int minCapturesCount)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, field, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;

            //no captures - simple move
            if (minCapturesCount > 0)
                return (new List<ICheckersAction>(), 0);
            List<ICheckersAction> possibleMoves = new();

            int? neighbour = CheckersState.Neighbours[0](field);
            if (neighbour is not null && state.GetPieceAt((int)neighbour) == Piece.None)
                possibleMoves.Add(new MoveAction(field, (int)neighbour));

            neighbour = CheckersState.Neighbours[1](field);
            if (neighbour is not null && state.GetPieceAt((int)neighbour) == Piece.None)
                possibleMoves.Add(new MoveAction(field, (int)neighbour));

            return (possibleMoves, 0);
        }

        private (List<ICheckersAction>, int) PossibleBlackPawnActions(
            CheckersState state, int field, int minCapturesCount)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, field, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;

            //no captures - simple move
            if (minCapturesCount > 0)
                return (new List<ICheckersAction>(), 0);
            List<ICheckersAction> possibleMoves = new();

            int? neighbour = CheckersState.Neighbours[2](field);
            if (neighbour is not null && state.GetPieceAt((int)neighbour) == Piece.None)
                possibleMoves.Add(new MoveAction(field, (int)neighbour));

            neighbour = CheckersState.Neighbours[3](field);
            if (neighbour is not null && state.GetPieceAt((int)neighbour) == Piece.None)
                possibleMoves.Add(new MoveAction(field, (int)neighbour));

            return (possibleMoves, 0);
        }

        private (List<ICheckersAction>, int) PossiblePawnCaptures(CheckersState state, int field, int capturesCount)
        {
            (List<ICheckersAction>, int) possibleCaptures = (new(), capturesCount);

            Piece capturer = state.GetPieceAt(field);
            for (int direction = 0; direction < 4; direction++)
            {
                int? neighbour = CheckersState.Neighbours[direction](field);
                if (neighbour is null)
                    continue;
                Piece target = state.GetPieceAt(neighbour.Value);
                if (HaveOppositeColors(capturer, target))
                {
                    int? finish = CheckersState.Neighbours[direction](neighbour.Value);
                    if (finish.HasValue && state.GetPieceAt(finish.Value) == Piece.None)
                    {
                        CaptureAction action = new(field, neighbour.Value, finish.Value);
                        CheckersState tmpState = PerformTemporaryCapture(action, state);
                        (List<ICheckersAction>, int) furtherCaptures = PossiblePawnCaptures(tmpState, finish.Value, capturesCount - 1);
                        furtherCaptures = CombineCaptures(action, furtherCaptures);
                        possibleCaptures = UpdateActions(possibleCaptures, furtherCaptures);
                    }
                }
            }
            return possibleCaptures;
        }

        private static (List<ICheckersAction>, int) PossibleCrownedActions(
            CheckersState state, int field, int capturesCount)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = (new(), capturesCount);
            List<ICheckersAction> possibleMoves = new();

            Piece movingPiece = state.GetPieceAt(field);
            for (int direction = 0; direction < 4; direction++)
            {
                int? targetedField = CheckersState.Neighbours[direction](field);
                while (targetedField.HasValue)
                {
                    Piece target = state.GetPieceAt(targetedField.Value);
                    if (target == Piece.None)
                    {
                        possibleMoves.Add(new MoveAction(field, targetedField.Value));
                    }
                    else // there is some piece on the way
                    {
                        if (HaveOppositeColors(movingPiece, target)) // there is some piece to capture
                        {
                            int? fieldBehindTarget = CheckersState.Neighbours[direction](targetedField.Value);
                            while (fieldBehindTarget.HasValue && state.GetPieceAt(fieldBehindTarget.Value) == Piece.None)
                            {
                                CaptureAction action = new(field, targetedField.Value, fieldBehindTarget.Value);
                                CheckersState tmpState = PerformTemporaryCapture(action, state);
                                (List<ICheckersAction>, int) furtherCaptures
                                    = PossibleCrownedCaptures(tmpState, fieldBehindTarget.Value, capturesCount - 1);
                                furtherCaptures = CombineCaptures(action, furtherCaptures);
                                possibleCaptures = UpdateActions(possibleCaptures, furtherCaptures);

                                fieldBehindTarget = CheckersState.Neighbours[direction](fieldBehindTarget.Value);
                            }
                        }
                        break;
                    }
                    targetedField = CheckersState.Neighbours[direction](targetedField.Value);
                }
            }
            if (possibleCaptures.list.Any())
                return possibleCaptures;

            //no captures found so far - simple moves through diagonals
            return (possibleMoves, 0);
        }

        private static (List<ICheckersAction>, int) PossibleCrownedCaptures(
            CheckersState state, int field, int minCapturesCount)
        {
            (List<ICheckersAction> list, int capturesCount) possibleCaptures = (new(), minCapturesCount);

            Piece movingPiece = state.GetPieceAt(field);
            for (int direction = 0; direction < 4; direction++)
            {
                int? targetedField = CheckersState.Neighbours[direction](field);
                while (targetedField.HasValue)
                {
                    Piece target = state.GetPieceAt(targetedField.Value);
                    if (target != Piece.None) // there is some piece on the way
                    {
                        if (HaveOppositeColors(movingPiece, target)) // there is some piece to capture
                        {
                            int? fieldBehindTarget = CheckersState.Neighbours[direction](targetedField.Value);
                            while (fieldBehindTarget.HasValue && state.GetPieceAt(fieldBehindTarget.Value) == Piece.None)
                            {
                                CaptureAction action = new(field, targetedField.Value, fieldBehindTarget.Value);
                                CheckersState tmpState = PerformTemporaryCapture(action, state);
                                (List<ICheckersAction>, int) furtherCaptures
                                    = PossibleCrownedCaptures(tmpState, fieldBehindTarget.Value, minCapturesCount - 1);
                                furtherCaptures = CombineCaptures(action, furtherCaptures);
                                possibleCaptures = UpdateActions(possibleCaptures, furtherCaptures);

                                fieldBehindTarget = CheckersState.Neighbours[direction](fieldBehindTarget.Value);
                            }
                        }
                        break;
                    }
                    targetedField = CheckersState.Neighbours[direction](targetedField.Value);
                }
            }
            return possibleCaptures;
        }

        private static (List<ICheckersAction> list, int capturesCount) CombineCaptures(
            CaptureAction action,
            (List<ICheckersAction> list, int capturesCount) furtherCaptures
            )
        {
            if (furtherCaptures.list.Any())
            {
                foreach (CaptureAction furtherCapture in furtherCaptures.list.Cast<CaptureAction>())
                {
                    furtherCapture.CombineCapture(action.Start, action.Captures.First!.Value.Captured);
                }
                furtherCaptures.capturesCount++;
                return furtherCaptures;
            }
            // only MoveActions found
            return (new List<ICheckersAction>() { action }, 1);
        }

        private static (List<ICheckersAction> list, int capturesCount) UpdateActions(
            (List<ICheckersAction> list, int capturesCount) possibleActions,
            (List<ICheckersAction> list, int capturesCount) newActions)
        {
            if (newActions.capturesCount == possibleActions.capturesCount) // add actions of same length
            {
                possibleActions.list.AddRange(newActions.list);
                return possibleActions;
            }
            if (newActions.capturesCount > possibleActions.capturesCount) // trim to longest actions only
            {
                return newActions;
            }
            // no changes
            return possibleActions;
        }

        private static bool HaveOppositeColors(Piece capturer, Piece target)
        {
            if (capturer == Piece.None || target == Piece.None) return false;
            if (capturer == Piece.CapturedPiece || target == Piece.CapturedPiece) return false;
            bool isCapturerWhite = capturer == Piece.WhitePawn || capturer == Piece.WhiteCrowned;
            bool isTargetBlack = target == Piece.BlackPawn || target == Piece.BlackCrowned;
            return (isCapturerWhite && isTargetBlack) || (!isCapturerWhite && !isTargetBlack);
        }
    }
}