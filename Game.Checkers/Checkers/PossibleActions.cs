using Game.IGame;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, ICheckersInputState>
    {
        public IEnumerable<CheckersAction> PossibleActions(CheckersState state)
        {
            (List<CheckersAction> list, int maxCapturesCount) possibleActions = (new(), 0);
            if (state.CurrentPlayer == Player.One)
            {
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.WhitePawn)
                        UpdateActions(ref possibleActions, PossibleWhitePawnActions(state, field, possibleActions.maxCapturesCount));
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
                        UpdateActions(ref possibleActions, PossibleBlackPownActions(state, field, possibleActions.maxCapturesCount));
                    else if (piece == Piece.BlackCrowned)
                        UpdateActions(ref possibleActions, PossibleCrownedActions(state, field, possibleActions.maxCapturesCount));

                }
            }
            return possibleActions.list;
        }

        private (List<CheckersAction>, int) PossibleWhitePawnActions(CheckersState state, Field start, int minCapturesCount)
        {
            (List<CheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, start, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;
            //no captures - simple move north-east/north-west
            if (minCapturesCount > 0)
                return (new List<CheckersAction>(), 0);
            List<CheckersAction> possibleMoves = new();
            int newRow = start.Row + 1;
            if (newRow < CheckersState.BOARD_SIZE)
            {
                int newCol = start.Col + 1;
                if (newCol < CheckersState.BOARD_SIZE && state.GetPieceAt(newCol, newRow) == Piece.None)
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

        private (List<CheckersAction>, int) PossibleBlackPownActions(CheckersState state, Field start, int minCapturesCount)
        {
            (List<CheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, start, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;
            //no captures - simple move south-east/south-west
            if (minCapturesCount > 0)
                return (new List<CheckersAction>(), 0);
            List<CheckersAction> possibleMoves = new();
            int newRow = start.Row - 1;
            if (newRow >= 0)
            {
                int newCol = start.Col + 1;
                if (newCol < CheckersState.BOARD_SIZE && state.GetPieceAt(newCol, newRow) == Piece.None)
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

        private (List<CheckersAction>, int) PossiblePawnCaptures(CheckersState state, Field start, int minCapturesCount)
        {
            (List<CheckersAction>, int) possibleCaptures = (new(), minCapturesCount);

            Piece capturer = state.GetPieceAt(start);
            IEnumerable<Field> neighbours = state.GetNeighbours(start);
            foreach (Field neighbour in neighbours)
            {
                Piece target = state.GetPieceAt(neighbour);
                if (HaveOppositeColors(capturer, target))
                {
                    (int x, int y) = (neighbour.Col - start.Col, neighbour.Row - start.Row);
                    Field finish = new(neighbour.Col + x, neighbour.Row + y);
                    if (finish.Col < CheckersState.BOARD_SIZE && finish.Col >= 0
                        && finish.Row < CheckersState.BOARD_SIZE && finish.Row >= 0
                        && state.GetPieceAt(finish) == Piece.None)
                    {
                        CaptureAction action = new(start, neighbour, finish);
                        CheckersState tmpState = PerformTemporaryCapture(action, state);
                        (List<CheckersAction>, int) furtherCaptures = PossiblePawnCaptures(tmpState, finish, minCapturesCount - 1);
                        CombineCaptures(action, ref furtherCaptures);
                        UpdateActions(ref possibleCaptures, furtherCaptures);
                    }
                }
            }
            return possibleCaptures;
        }

        private static (List<CheckersAction>, int) PossibleCrownedActions(CheckersState state, Field start, int minCapturesCount)
        {
            (List<CheckersAction> list, int maxCapturesCount) possibleCaptures = PossibleCrownedCaptures(state, start, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;
            //no captures - simple moves through diagonals
            if (minCapturesCount > 0)
                return (new List<CheckersAction>(), 0);
            return (PossibleCrownedMoves(state, start), 0);
        }

        private static (List<CheckersAction>, int) PossibleCrownedCaptures(CheckersState state, Field start, int minCapturesCount)
        {
            (List<CheckersAction>, int) possibleCaptures = (new(), minCapturesCount);
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
                            (List<CheckersAction>, int) furtherCaptures = PossibleCrownedCaptures(tmpState, finish, minCapturesCount - 1);
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

        private static List<CheckersAction> PossibleCrownedMoves(CheckersState state, Field start)
        {
            List<CheckersAction> possibleMoves = new();
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
            ref (List<CheckersAction> list, int maxCapturesCount) furtherCaptures)
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
            furtherCaptures = (new List<CheckersAction>() { action }, 1);
            return false;
        }

        private static bool UpdateActions(
            ref (List<CheckersAction> list, int maxCapturesCount) possibleActions,
            (List<CheckersAction> list, int maxCapturesCount) newActions)
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
            yield return (1, 1, Math.Min(CheckersState.BOARD_SIZE - 1 - field.Col, CheckersState.BOARD_SIZE - 1 - field.Row));
            yield return (-1, 1, Math.Min(field.Col, CheckersState.BOARD_SIZE - 1 - field.Row));
            yield return (1, -1, Math.Min(CheckersState.BOARD_SIZE - 1 - field.Col, field.Row));
            yield return (-1, -1, Math.Min(field.Col, field.Row));
        }
    }
}