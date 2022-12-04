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
            foreach(Field field in action.ConsecutiveFields)
            {
                newState.SetPieceAt(field.X, field.Y, field.OccupiedBy);
            }
            return newState;
        }

        public IEnumerable<CheckersAction> PossibleActions(CheckersState state)
        {
            List<CheckersAction> possibleActions = new List<CheckersAction>();
            if (state.CurrentPlayer == Player.White)
            {
                for (int x = 0; x < CheckersState.BOARD_ROWS; x++)
                {
                    for (int y = 0; y < CheckersState.BOARD_COLS; y++)
                    {
                        if (state.GetPieceAt((x, y)) == Piece.WhitePawn)
                            possibleActions.AddRange(PossibleWhitePawnActions(state, (x, y)));
                        else if (state.GetPieceAt((x, y)) == Piece.WhiteCrowned)
                            possibleActions.AddRange(PossibleWhiteCrownedActions(state, (x, y)));
                    }
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                for (int x = 0; x < CheckersState.BOARD_ROWS; x++)
                {
                    for (int y = 0; y < CheckersState.BOARD_COLS; y++)
                    {
                        if (state.GetPieceAt((x, y)) == Piece.BlackPawn)
                            possibleActions.AddRange(PossibleBlackPownActions(state, (x, y)));
                        else if (state.GetPieceAt((x, y)) == Piece.BlackCrowned)
                            possibleActions.AddRange(PossibleBlackCrownedActions(state, (x, y)));
                    }
                }
            }
            return possibleActions;
        }

        private IEnumerable<CheckersAction> PossibleBlackCrownedActions(CheckersState state, (int x, int y) piecePosition)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CheckersAction> PossibleBlackPownActions(CheckersState state, (int x, int y) start)
        {
            List<CheckersAction> possibleActions = new List<CheckersAction>();
            IEnumerable<List<Field>> possibleCaptures = PossiblePawnCaptures(state, start);
            if (possibleCaptures.Count() > 0)
            {
                foreach (var capture in possibleCaptures)
                {
                    if (capture.Last().Y == 0) // finnished action on the last line - crowning
                    {
                        Field lastField = capture.Last();
                        capture.Remove(lastField);
                        lastField.OccupiedBy = Piece.BlackCrowned;
                        capture.Add(lastField);
                    }
                    possibleActions.Add(new CheckersAction(capture));
                }
                return possibleActions;
            }
            //no captures
            int x = start.x;
            int y = start.y;
            if (y == 0) // finnished action on the last line - crowning
            {
                if (x < CheckersState.BOARD_COLS - 1)
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x + 1, y + 1, Piece.BlackCrowned) }));
                if (x > 0)
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x - 1, y + 1, Piece.BlackCrowned) }));
            }
            else if (y > 0)
            {
                if (x < CheckersState.BOARD_COLS - 1)
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x + 1, y + 1, Piece.BlackPawn) }));
                if (x > 0)
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x - 1, y + 1, Piece.BlackPawn) }));
            }
            return possibleActions;
        }

        private IEnumerable<CheckersAction> PossibleWhiteCrownedActions(CheckersState state, (int x, int y) piecePosition)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CheckersAction> PossibleWhitePawnActions(CheckersState state, (int x, int y) start)
        {
            List<CheckersAction> possibleActions = new List<CheckersAction>();
            IEnumerable<List<Field>> possibleCaptures = PossiblePawnCaptures(state, start);
            if (possibleCaptures.Count() > 0)
            {
                foreach (var capture in possibleCaptures)
                {
                    if (capture.Last().Y == CheckersState.BOARD_ROWS - 1) // finnished action on the last line - crowning
                    {
                        Field lastField = capture.Last();
                        capture.Remove(lastField);
                        lastField.OccupiedBy = Piece.WhiteCrowned;
                        capture.Add(lastField);
                    }
                    possibleActions.Add(new CheckersAction(capture));
                }
                return possibleActions;
            }
            //no captures - simple move forward
            int x = start.x;
            int y = start.y;
            if (x < CheckersState.BOARD_COLS - 1 && state.GetPieceAt((x + 1, y + 1)) == Piece.None)
            {
                if (y == CheckersState.BOARD_ROWS - 1) // finnished action on the last line - crowning
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x + 1, y + 1, Piece.WhiteCrowned) }));
                else if (y < CheckersState.BOARD_ROWS - 2)
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x + 1, y + 1, Piece.WhitePawn) }));
            }
            if (x > 0 && state.GetPieceAt((x - 1, y + 1)) == Piece.None)
            {
                if (y == CheckersState.BOARD_ROWS - 1) // finnished action on the last line - crowning
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x - 1, y + 1, Piece.WhiteCrowned) }));
                else if (y < CheckersState.BOARD_ROWS - 2)
                    possibleActions.Add(new CheckersAction(new List<Field> {
                            new Field(x, y, Piece.None), new Field(x - 1, y + 1, Piece.WhitePawn) }));
            }

            return possibleActions;
        }

        private List<List<Field>> PossiblePawnCaptures(CheckersState state, (int x, int y) start)
        {
            List<List<Field>> possibleCaptures = new List<List<Field>>();
            IEnumerable<(int x, int y)> neighbours = GetNeighbours(start);
            Piece capturer = state.GetPieceAt(start);
            foreach ((int x, int y) neighbour in neighbours)
            {
                Piece target = state.GetPieceAt(neighbour);
                if (HaveOppositeColors(capturer, target))
                {
                    (int x, int y) vector = (neighbour.x - start.x, neighbour.y - start.y);
                    (int x, int y) endPosition = (neighbour.x + vector.x, neighbour.y + vector.y);
                    if (endPosition.x < CheckersState.BOARD_COLS && endPosition.x >= 0
                        && endPosition.y < CheckersState.BOARD_ROWS && endPosition.y >= 0
                        && state.GetPieceAt(endPosition) == Piece.None)
                    {
                        List<Field> fieldsAfterCapture = new List<Field> {
                            new Field(start.x, start.y, Piece.None),
                            new Field(neighbour.x, neighbour.y, Piece.None),
                            new Field(endPosition.x, endPosition.y, capturer),
                        };
                        Field[] tmpFieldsAfterCapture = new Field[] {
                            fieldsAfterCapture[0],
                            new Field(neighbour.x, neighbour.y, Piece.Captured),
                            fieldsAfterCapture[2],
                        };
                        CheckersState newState = PerformAction(new CheckersAction(tmpFieldsAfterCapture), state);
                        IEnumerable<List<Field>> furtherCaptures = PossiblePawnCaptures(newState, endPosition);
                        if (furtherCaptures.Any())
                        {
                            List<List<Field>> combinedCaptures = CombineCaptures(fieldsAfterCapture[0],
                                fieldsAfterCapture[0], furtherCaptures);
                            possibleCaptures.AddRange(combinedCaptures);
                        }
                        else possibleCaptures.Add(fieldsAfterCapture);
                    }
                }
            }
            // trim to longest captures only
            int? maxCapture = possibleCaptures.MaxBy(capture => capture.Count())?.Count();
            if (maxCapture != null)
                possibleCaptures = possibleCaptures.FindAll(capture => capture.Count() == maxCapture);
            return possibleCaptures;
        }

        private List<List<Field>> CombineCaptures(Field field1, Field field2, IEnumerable<List<Field>> furtherCaptures)
        {
            List<List<Field>> combinedCaptures = new List<List<Field>>();
            foreach (List<Field> furtherCapture in furtherCaptures)
            {
                List<Field> combinedCapture = new List<Field> { field1, field2 };
                combinedCapture.AddRange(furtherCapture);
                combinedCaptures.Add(combinedCapture);
            }
            return combinedCaptures;
        }

        private IEnumerable<(int x, int y)> GetNeighbours((int x, int y) position)
        {
            int x = position.x;
            int y = position.y;
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

        private bool HaveOppositeColors(Piece capturer, Piece target)
        {
            if (capturer == Piece.None || target == Piece.None) return false;
            if (capturer == Piece.Captured || target == Piece.Captured) return false;
            bool isCapturerWhite = capturer == Piece.WhitePawn || capturer == Piece.None;
            bool isTargetBlack = target == Piece.WhitePawn || target == Piece.None;
            return (isCapturerWhite && isTargetBlack) || (!isCapturerWhite && !isTargetBlack);
        }
    }
}