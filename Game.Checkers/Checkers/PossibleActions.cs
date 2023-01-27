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
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.WhitePawn)
                        UpdateActions(ref possibleActions, PossiblePawnActions(state, field, possibleActions.maxCapturesCount, 1));
                    else if (piece == Piece.WhiteCrowned)
                        UpdateActions(ref possibleActions, PossibleCrownedActions(state, field, possibleActions.maxCapturesCount));
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.BlackPawn)
                        UpdateActions(ref possibleActions, PossiblePawnActions(state, field, possibleActions.maxCapturesCount, -1));
                    else if (piece == Piece.BlackCrowned)
                        UpdateActions(ref possibleActions, PossibleCrownedActions(state, field, possibleActions.maxCapturesCount));

                }
            }
            return possibleActions.list;
        }

        private (List<ICheckersAction>, int) PossiblePawnActions(
            CheckersState state,
            Field start,
            int minCapturesCount,
            int directionOfMovement)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, start, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;
            //no captures - simple move
            if (minCapturesCount > 0)
                return (new List<ICheckersAction>(), 0);
            List<ICheckersAction> possibleMoves = new();
            int newRow = start.Row + directionOfMovement;
            if (newRow < CheckersState.boardSize)
            {
                int newCol = start.Col + 1;
                if (newCol < CheckersState.boardSize && state.GetPieceAt(newCol, newRow) == Piece.None)
                {
                    possibleMoves.Add(new MoveAction(start, new Field(newCol, newRow)));
                }
                newCol = start.Col - 1;
                if (newCol >= 0 && state.GetPieceAt(newCol, newRow) == Piece.None)
                {
                    possibleMoves.Add(new MoveAction(start, new Field(newCol, newRow)));
                }
            }
            return (possibleMoves, 0);
        }

        private (List<ICheckersAction>, int) PossiblePawnCaptures(
            CheckersState state,
            Field start,
            int minCapturesCount)
        {
            (List<ICheckersAction>, int) possibleCaptures = (new(), minCapturesCount);

            Piece capturer = state.GetPieceAt(start);
            IEnumerable<Field> neighbours = state.GetNeighbours(start);
            foreach (Field neighbour in neighbours)
            {
                Piece target = state.GetPieceAt(neighbour);
                if (HaveOppositeColors(capturer, target))
                {
                    (int x, int y) = (neighbour.Col - start.Col, neighbour.Row - start.Row);
                    Field finish = new(neighbour.Col + x, neighbour.Row + y);
                    if (finish.Col < CheckersState.boardSize && finish.Col >= 0
                        && finish.Row < CheckersState.boardSize && finish.Row >= 0
                        && state.GetPieceAt(finish) == Piece.None)
                    {
                        CaptureAction action = new(start, neighbour, finish);
                        CheckersState tmpState = PerformTemporaryCapture(action, state);
                        (List<ICheckersAction>, int) furtherCaptures = PossiblePawnCaptures(tmpState, finish, minCapturesCount - 1);
                        CombineCaptures(action, ref furtherCaptures);
                        UpdateActions(ref possibleCaptures, furtherCaptures);
                    }
                }
            }
            return possibleCaptures;
        }

        private static (List<ICheckersAction>, int) PossibleCrownedActions(
            CheckersState state,
            Field start,
            int minCapturesCount)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = PossibleCrownedCaptures(state, start, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;
            //no captures - simple moves through diagonals
            if (minCapturesCount > 0)
                return (new List<ICheckersAction>(), 0);
            return (PossibleCrownedMoves(state, start), 0);
        }

        private static (List<ICheckersAction>, int) PossibleCrownedCaptures(
            CheckersState state,
            Field start,
            int minCapturesCount)
        {
            (List<ICheckersAction>, int) possibleCaptures = (new(), minCapturesCount);
            foreach ((int dCol, int dRow, int fields) in GetDiagsData(start))
            {
                int col = start.Col + dCol;
                int row = start.Row + dRow;
                int fieldsToCheck = fields;
                while (fieldsToCheck > 1 && state.GetPieceAt(col, row) == Piece.None)
                {
                    col += dCol;
                    row += dRow;
                    fieldsToCheck--;
                }
                if (fieldsToCheck > 1)
                {
                    Piece capturer = state.GetPieceAt(start);
                    Piece captured = state.GetPieceAt(col, row);
                    if (HaveOppositeColors(capturer, captured))
                    {
                        Field target = new(col, row);
                        col += dCol;
                        row += dRow;
                        fieldsToCheck--;
                        while (fieldsToCheck > 0 && state.GetPieceAt(col, row) == Piece.None)
                        {
                            Field finish = new(col, row);
                            CaptureAction action = new(start, target, finish);
                            CheckersState tmpState = PerformTemporaryCapture(action, state);
                            (List<ICheckersAction>, int) furtherCaptures = PossibleCrownedCaptures(tmpState, finish, minCapturesCount - 1);
                            CombineCaptures(action, ref furtherCaptures);
                            UpdateActions(ref possibleCaptures, furtherCaptures);

                            col += dCol;
                            row += dRow;
                            fieldsToCheck--;
                        }
                    }
                }
            }
            return possibleCaptures;
        }

        private static List<ICheckersAction> PossibleCrownedMoves(CheckersState state, Field start)
        {
            List<ICheckersAction> possibleMoves = new();
            foreach ((int dCol, int dRow, int fields) in GetDiagsData(start))
            {
                int col = start.Col + dCol;
                int row = start.Row + dRow;
                int fieldsToCheck = fields;
                while (fieldsToCheck > 0 && state.GetPieceAt(col, row) == Piece.None)
                {
                    possibleMoves.Add(new MoveAction(start, new(col, row)));
                    col += dCol;
                    row += dRow;
                    fieldsToCheck--;
                }
            }
            return possibleMoves;
        }

        private static bool CombineCaptures(CaptureAction action,
            ref (List<ICheckersAction> list, int maxCapturesCount) furtherCaptures)
        {
            if (furtherCaptures.list.Any())
            {
                foreach (CaptureAction furtherCapture in furtherCaptures.list.Cast<CaptureAction>())
                {
                    furtherCapture.CombineCapture(action.Start, action.Captures.First!.Value.Captured);
                }
                furtherCaptures.maxCapturesCount++;
                return true;
            }
            furtherCaptures = (new List<ICheckersAction>() { action }, 1);
            return false;
        }

        private static bool UpdateActions(
            ref (List<ICheckersAction> list, int maxCapturesCount) possibleActions,
            (List<ICheckersAction> list, int maxCapturesCount) newActions)
        {
            if (newActions.maxCapturesCount == possibleActions.maxCapturesCount) // add actions of same length
            {
                possibleActions.list.AddRange(newActions.list);
                return true;
            }
            if (newActions.maxCapturesCount > possibleActions.maxCapturesCount) // trim to longest actions only
            {
                possibleActions = newActions;
                return true;
            }
            return false; // no changes
        }

        private static bool HaveOppositeColors(Piece capturer, Piece target)
        {
            if (capturer == Piece.None || target == Piece.None) return false;
            if (capturer == Piece.CapturedPiece || target == Piece.CapturedPiece) return false;
            bool isCapturerWhite = capturer == Piece.WhitePawn || capturer == Piece.WhiteCrowned;
            bool isTargetBlack = target == Piece.BlackPawn || target == Piece.BlackCrowned;
            return (isCapturerWhite && isTargetBlack) || (!isCapturerWhite && !isTargetBlack);
        }

        private static IEnumerable<(int, int, int)> GetDiagsData(Field field)
        {
            yield return (1, 1, Math.Min(CheckersState.boardSize - 1 - field.Col, CheckersState.boardSize - 1 - field.Row));
            yield return (-1, 1, Math.Min(field.Col, CheckersState.boardSize - 1 - field.Row));
            yield return (1, -1, Math.Min(CheckersState.boardSize - 1 - field.Col, field.Row));
            yield return (-1, -1, Math.Min(field.Col, field.Row));
        }
    }
}