using Game.IGame;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, CheckersInputState>
    {
        public IEnumerable<CheckersAction> PossibleActions(CheckersState state)
        {
            (List<CheckersAction> list, int maxCapturesCount) possibleActions = (new(), 0);
            if (state.CurrentPlayer == Player.White)
            {
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.WhitePawn)
                        AddNewActions(ref possibleActions, PossibleWhitePawnActions(state, field));
                    else if (piece == Piece.WhiteCrowned)
                        AddNewActions(ref possibleActions, PossibleCrownedActions(state, field));
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.BlackPawn)
                        AddNewActions(ref possibleActions, PossibleBlackPownActions(state, field));
                    else if (piece == Piece.BlackCrowned)
                        AddNewActions(ref possibleActions, PossibleCrownedActions(state, field));

                }
            }
            return possibleActions.list;
        }

        private (IEnumerable<CheckersAction>, int) PossibleWhitePawnActions(CheckersState state, Field start)
        {
            (IEnumerable<CheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, start);
            if (possibleCaptures.list.Any())
                return possibleCaptures;
            //no captures - simple move north-east/north-west
            List<MoveAction> possibleMoves = new();
            int newRow = start.Row + 1;
            if (newRow < CheckersState.BOARD_ROWS)
            {
                int newCol = start.Col + 1;
                if (newCol < CheckersState.BOARD_COLS && state.GetPieceAt(newCol, newRow) == Piece.None)
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

        private (IEnumerable<CheckersAction>, int) PossibleBlackPownActions(CheckersState state, Field start)
        {
            (IEnumerable<CheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, start);
            if (possibleCaptures.list.Any())
                return possibleCaptures;
            //no captures - simple move south-east/south-west
            List<MoveAction> possibleMoves = new();
            int newRow = start.Row - 1;
            if (newRow >= 0)
            {
                int newCol = start.Col + 1;
                if (newCol < CheckersState.BOARD_COLS && state.GetPieceAt(newCol, newRow) == Piece.None)
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

        private (IEnumerable<CheckersAction>, int) PossiblePawnCaptures(CheckersState state, Field start)
        {
            (List<CheckersAction>, int) possibleCaptures = (new(), 0);

            Piece capturer = state.GetPieceAt(start);
            IEnumerable<Field> neighbours = state.GetNeighbours(start);
            foreach (Field neighbour in neighbours)
            {
                Piece target = state.GetPieceAt(neighbour);
                if (HaveOppositeColors(capturer, target))
                {
                    (int x, int y) = (neighbour.Col - start.Col, neighbour.Row - start.Row);
                    Field finnish = new(neighbour.Col + x, neighbour.Row + y);
                    if (finnish.Col < CheckersState.BOARD_COLS && finnish.Col >= 0
                        && finnish.Row < CheckersState.BOARD_ROWS && finnish.Row >= 0
                        && state.GetPieceAt(finnish) == Piece.None)
                    {
                        CaptureAction action = new(start, neighbour, finnish);
                        CheckersState tmpState = PerformTemporaryCapture(action, state);
                        (IEnumerable<CheckersAction>, int) furtherCaptures = PossiblePawnCaptures(tmpState, finnish);
                        CombineCaptures(action, ref furtherCaptures);
                        AddNewActions(ref possibleCaptures, furtherCaptures);
                    }
                }
            }
            return possibleCaptures;
        }

        private static (IEnumerable<CheckersAction>, int) PossibleCrownedActions(CheckersState state, Field start)
        {
            (IEnumerable<CheckersAction> list, int maxCapturesCount) possibleCaptures = PossibleCrownedCaptures(state, start);
            if (possibleCaptures.list.Any())
                return possibleCaptures;
            //no captures - simple moves through diagonals
            return (PossibleCrownedMoves(state, start), 0);
        }

        private static (IEnumerable<CheckersAction>, int) PossibleCrownedCaptures(CheckersState state, Field start)
        {
            (List<CheckersAction>, int) possibleCaptures = (new(), 0);
            foreach ((int dCol, int dRow, int fields) in GetDiagsData(start))
            {
                int col = start.Col + dCol;
                int row = start.Row + dRow;
                int fieldsToCheck = fields;
                while (fieldsToCheck > 1 && state.GetPieceAt(col, row) == Piece.None)
                {
                    col += dCol;
                    row += dCol;
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
                        row += dCol;
                        fieldsToCheck--;
                        while (fieldsToCheck > 0 && state.GetPieceAt(col, row) == Piece.None)
                        {
                            Field finnish = new(col, row);
                            CaptureAction action = new(start, target, finnish);
                            CheckersState tmpState = PerformTemporaryCapture(action, state);
                            (IEnumerable<CheckersAction>, int) furtherCaptures = PossibleCrownedCaptures(tmpState, finnish);
                            CombineCaptures(action, ref furtherCaptures);
                            AddNewActions(ref possibleCaptures, furtherCaptures);

                            col += dCol;
                            row += dCol;
                            fieldsToCheck--;
                        }
                    }
                }
            }
            return possibleCaptures;
        }

        private static IEnumerable<CheckersAction> PossibleCrownedMoves(CheckersState state, Field start)
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
            ref (IEnumerable<CheckersAction> list, int maxCapturesCount) furtherCaptures)
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
            furtherCaptures = (new List<CaptureAction>() { action }, 1);
            return false;
        }

        private static bool AddNewActions(
            ref (List<CheckersAction> list, int maxCapturesCount) possibleActions,
            (IEnumerable<CheckersAction> list, int maxCapturesCount) newActions)
        {
            if (newActions.maxCapturesCount == possibleActions.maxCapturesCount) // add actions of same length
            {
                possibleActions.list.AddRange(newActions.list);
                return true;
            }
            if (newActions.maxCapturesCount > possibleActions.maxCapturesCount) // trim to longest actions only
            {
                possibleActions.maxCapturesCount = newActions.maxCapturesCount;
                possibleActions.list.Clear();
                possibleActions.list.AddRange(newActions.list);
                return true;
            }
            return false;
        }

        private static bool HaveOppositeColors(Piece capturer, Piece target)
        {
            if (capturer == Piece.None || target == Piece.None) return false;
            if (capturer == Piece.Captured || target == Piece.Captured) return false;
            bool isCapturerWhite = capturer == Piece.WhitePawn || capturer == Piece.WhiteCrowned;
            bool isTargetBlack = target == Piece.BlackPawn || target == Piece.BlackCrowned;
            return (isCapturerWhite && isTargetBlack) || (!isCapturerWhite && !isTargetBlack);
        }

        private static IEnumerable<(int, int, int)> GetDiagsData(Field field)
        {
            yield return (1, 1, Math.Min(CheckersState.BOARD_COLS - 1 - field.Col, CheckersState.BOARD_ROWS - 1 - field.Row));
            yield return (-1, 1, Math.Min(field.Col, CheckersState.BOARD_ROWS - 1 - field.Row));
            yield return (1, -1, Math.Min(CheckersState.BOARD_COLS - 1 - field.Col, field.Row));
            yield return (-1, -1, Math.Min(field.Col, field.Row));
        }
    }
}