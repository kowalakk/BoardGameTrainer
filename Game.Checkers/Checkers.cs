using Game.IGame;
using Gdk;
using Gtk;

namespace Game.Checkers
{
    public class Checkers : IGame<CheckersAction, CheckersState, CheckersInputState>
    {
        public void DrawBoard(Widget widget, CheckersInputState inputState, CheckersState state, IEnumerable<(CheckersAction, double)> ratedActions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CheckersAction> FilterByInputState(IEnumerable<CheckersAction> actions, CheckersInputState inputState)
        {
            throw new NotImplementedException();
        }

        //needs optimization
        public GameResults GameResult(CheckersState state)
        {
            IEnumerable<CheckersAction> possibleActions = PossibleActions(state);
            if (possibleActions.Count() == 0)
            {
                if (state.CurrentPlayer == Player.White) return GameResults.PlayerTwoWins;
                return GameResults.PlayerOneWins;
            }
            return GameResults.InProgress;
        }

        public (CheckersAction, CheckersInputState) HandleInput(Event @event, CheckersInputState inputState, CheckersState state)
        {
            throw new NotImplementedException();
        }

        public CheckersState PerformAction(CheckersAction action, CheckersState state)
        {
            CheckersState newState = new CheckersState(state);
            foreach (Field field in action.ConsecutiveFields)
            {
                newState.SetPieceAt(field.X, field.Y, field.OccupiedBy);
            }
            return newState;
        }

        public IEnumerable<CheckersAction> PossibleActions(CheckersState state)
        {
            List<CheckersAction> possibleActions = new();
            if (state.CurrentPlayer == Player.White)
            {
                foreach (Field field in state.GetFields())
                {
                    if (field.OccupiedBy == Piece.WhitePawn)
                        possibleActions.AddRange(PossibleWhitePawnActions(state, field));
                    else if (field.OccupiedBy == Piece.WhiteCrowned)
                        possibleActions.AddRange(PossibleWhiteCrownedActions(state, field));
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                foreach (Field field in state.GetFields())
                {
                    if (field.OccupiedBy == Piece.BlackPawn)
                        possibleActions.AddRange(PossibleBlackPownActions(state, field));
                    else if (field.OccupiedBy == Piece.BlackCrowned)
                        possibleActions.AddRange(PossibleBlackCrownedActions(state, field));
                }
            }
            return possibleActions;
        }

        private IEnumerable<CheckersAction> PossibleBlackCrownedActions(CheckersState state, Field start)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CheckersAction> PossibleBlackPownActions(CheckersState state, Field start)
        {
            List<CheckersAction> possibleActions = new();
            IEnumerable<List<Field>> possibleCaptures = PossiblePawnCaptures(state, start);
            if (possibleCaptures.Any())
            {
                foreach (var capture in possibleCaptures)
                {
                    possibleActions.Add(new CheckersAction(capture));
                }
                return possibleActions;
            }
            //no captures
            int x = start.X;
            int y = start.Y;
            if (y > 0)
            {
                if (x < CheckersState.BOARD_COLS - 1 && state.GetPieceAt(x + 1, y - 1) == Piece.None)
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x + 1, y - 1, Piece.BlackPawn) }));
                if (x > 0 && state.GetPieceAt(x - 1, y - 1) == Piece.None)
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x - 1, y - 1, Piece.BlackPawn) }));
            }
            return possibleActions;
        }

        private IEnumerable<CheckersAction> PossibleWhiteCrownedActions(CheckersState state, Field start)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CheckersAction> PossibleWhitePawnActions(CheckersState state, Field start)
        {
            List<CheckersAction> possibleActions = new List<CheckersAction>();
            IEnumerable<List<Field>> possibleCaptures = PossiblePawnCaptures(state, start);
            if (possibleCaptures.Any())
            {
                foreach (var capture in possibleCaptures)
                {
                    possibleActions.Add(new CheckersAction(capture));
                }
                return possibleActions;
            }
            //no captures - simple move forward
            int x = start.X;
            int y = start.Y;
            if (y < CheckersState.BOARD_ROWS - 1)
            {
                if (x < CheckersState.BOARD_COLS - 1 && state.GetPieceAt(x + 1, y + 1) == Piece.None)
                {
                        possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x + 1, y + 1, Piece.WhitePawn) }));
                }
                if (x > 0 && state.GetPieceAt(x - 1, y + 1) == Piece.None)
                {
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x - 1, y + 1, Piece.WhitePawn) }));
                }
            }
            return possibleActions;
        }

        private List<List<Field>> PossiblePawnCaptures(CheckersState state, Field start)
        {
            List<List<Field>> possibleCaptures = new();
            IEnumerable<(int x, int y)> neighbours = GetNeighbours(start.X, start.Y);
            Piece capturer = start.OccupiedBy;
            foreach ((int x, int y) neighbour in neighbours)
            {
                Piece target = state.GetPieceAt(neighbour);
                if (HaveOppositeColors(capturer, target))
                {
                    (int x, int y) vector = (neighbour.x - start.X, neighbour.y - start.Y);
                    (int x, int y) endPosition = (neighbour.x + vector.x, neighbour.y + vector.y);
                    if (endPosition.x < CheckersState.BOARD_COLS && endPosition.x >= 0
                        && endPosition.y < CheckersState.BOARD_ROWS && endPosition.y >= 0
                        && state.GetPieceAt(endPosition) == Piece.None)
                    {
                        List<Field> fieldsAfterCapture = new List<Field> {
                            new Field(start.X, start.Y, Piece.None),
                            new Field(neighbour.x, neighbour.y, Piece.None),
                            new Field(endPosition.x, endPosition.y, capturer),
                        };
                        Field[] tmpFieldsAfterCapture = new Field[] {
                            fieldsAfterCapture[0],
                            new Field(neighbour.x, neighbour.y, Piece.Captured),
                            fieldsAfterCapture[2],
                        };
                        CheckersState newState = PerformAction(new CheckersAction(tmpFieldsAfterCapture), state);
                        IEnumerable<List<Field>> furtherCaptures = PossiblePawnCaptures(newState, fieldsAfterCapture[2]);
                        if (furtherCaptures.Any())
                        {
                            List<List<Field>> combinedCaptures = CombineCaptures(fieldsAfterCapture[0],
                                fieldsAfterCapture[1], furtherCaptures);
                            possibleCaptures.AddRange(combinedCaptures);
                        }
                        else possibleCaptures.Add(fieldsAfterCapture);
                    }
                }
            }
            // trim to longest captures only
            int? maxCapture = possibleCaptures.MaxBy(capture => capture.Count)?.Count;
            if (maxCapture != null)
                possibleCaptures = possibleCaptures.FindAll(capture => capture.Count == maxCapture);
            return possibleCaptures;
        }

        private static List<List<Field>> CombineCaptures(Field field1, Field field2, IEnumerable<List<Field>> furtherCaptures)
        {
            List<List<Field>> combinedCaptures = new();
            foreach (List<Field> furtherCapture in furtherCaptures)
            {
                List<Field> combinedCapture = new() { field1, field2 };
                combinedCapture.AddRange(furtherCapture);
                combinedCaptures.Add(combinedCapture);
            }
            return combinedCaptures;
        }

        private static IEnumerable<(int x, int y)> GetNeighbours(int x, int y)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            if (y < CheckersState.BOARD_ROWS - 1)
            {
                if (x < CheckersState.BOARD_COLS - 1)
                    neighbours.Add((x + 1, y + 1));
                if (x > 0)
                    neighbours.Add((x - 1, y + 1));
            }
            if (y > 0)
            {
                if (x < CheckersState.BOARD_COLS - 1)
                    neighbours.Add((x + 1, y - 1));
                if (x > 0)
                    neighbours.Add((x - 1, y - 1));
            }
            return neighbours;
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