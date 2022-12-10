using Game.IGame;
using Gdk;
using Gtk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, CheckersInputState>
    {
        public IEnumerable<CheckersAction> PossibleActions(CheckersState state)
        {
            List<CheckersAction> possibleActions = new();
            if (state.CurrentPlayer == Player.White)
            {
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.WhitePawn)
                        possibleActions.AddRange(PossibleWhitePawnActions(state, field));
                    else if (piece == Piece.WhiteCrowned)
                        possibleActions.AddRange(PossibleCrownedActions(state, field));
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                foreach (Field field in state.GetFields())
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.BlackPawn)
                        possibleActions.AddRange(PossibleBlackPownActions(state, field));
                    else if (piece == Piece.BlackCrowned)
                        possibleActions.AddRange(PossibleCrownedActions(state, field));
                }
            }
            return possibleActions;
        }

        private IEnumerable<CheckersAction> PossibleWhitePawnActions(CheckersState state, Field start)
        {
            IEnumerable<CaptureAction> possibleCaptures = PossiblePawnCaptures(state, start);
            if (possibleCaptures.Any())
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
            return possibleMoves;
        }

        private IEnumerable<CheckersAction> PossibleBlackPownActions(CheckersState state, Field start)
        {
            IEnumerable<CaptureAction> possibleCaptures = PossiblePawnCaptures(state, start);
            if (possibleCaptures.Any())
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
            return possibleMoves;
        }

        private IEnumerable<CaptureAction> PossiblePawnCaptures(CheckersState state, Field start)
        {
            List<CaptureAction> possibleCaptures = new();
            IEnumerable<Field> neighbours = state.GetNeighbours(start);
            Piece capturer = state.GetPieceAt(start);
            foreach (Field neighbour in neighbours)
            {
                Piece target = state.GetPieceAt(neighbour);
                if (HaveOppositeColors(capturer, target))
                {
                    (int x, int y) = (neighbour.Col - start.Col, neighbour.Row - start.Row);
                    Field end = new(neighbour.Col + x, neighbour.Row + y);
                    if (end.Col < CheckersState.BOARD_COLS && end.Col >= 0
                        && end.Row < CheckersState.BOARD_ROWS && end.Row >= 0
                        && state.GetPieceAt(end) == Piece.None)
                    {
                        CaptureAction action = new(start, neighbour, end);
                        CheckersState tmpState = PerformTemporaryCapture(action, state);
                        IEnumerable<CaptureAction> furtherCaptures = PossiblePawnCaptures(tmpState, end);
                        if (furtherCaptures.Any())
                        {
                            CombineCaptures(start, neighbour, furtherCaptures);
                            possibleCaptures.AddRange(furtherCaptures);
                        }
                        else possibleCaptures.Add(action);
                    }
                }
            }
            // trim to longest captures only
            int? longestCapture = possibleCaptures.MaxBy(capture => capture.CapturesCount)?.CapturesCount;
            if (longestCapture != null)
                possibleCaptures = possibleCaptures.FindAll(capture => capture.CapturesCount == longestCapture);
            return possibleCaptures;
        }

        private IEnumerable<CheckersAction> PossibleCrownedActions(CheckersState state, Field start)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<CaptureAction> CombineCaptures(Field start, Field firstCapture, IEnumerable<CaptureAction> furtherCaptures)
        {
            foreach (CaptureAction furtherCapture in furtherCaptures)
            {
                furtherCapture.CombineCapture(start, firstCapture);
            }
            return furtherCaptures;
        }

        private static bool HaveOppositeColors(Piece capturer, Piece target)
        {
            if (capturer == Piece.None || target == Piece.None) return false;
            if (capturer == Piece.Captured || target == Piece.Captured) return false;
            bool isCapturerWhite = capturer == Piece.WhitePawn || capturer == Piece.WhiteCrowned;
            bool isTargetBlack = target == Piece.BlackPawn || target == Piece.BlackCrowned;
            return (isCapturerWhite && isTargetBlack) || (!isCapturerWhite && !isTargetBlack);
        }
    }
}