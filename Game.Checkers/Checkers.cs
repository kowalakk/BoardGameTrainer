using Game.IGame;
using Gdk;
using Gtk;
using System.Collections.Generic;

namespace Game.Checkers
{
    public class Checkers : IGame<CheckersAction, CheckersState, CheckersInputState>
    {
        public int BOARD_ROWS = 8;
        public int BOARD_COLS = 8;
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
            throw new NotImplementedException();
        }

        public IEnumerable<CheckersAction> PossibleActions(CheckersState state)
        {
            List<CheckersAction> possibleActions = new List<CheckersAction>();
            if (state.CurrentPlayer == Player.White)
            {
                for (int x = 0; x < BOARD_ROWS; x++)
                {
                    for (int y = 0; y < BOARD_COLS; y++)
                    {
                        if (state.GetPieceAt((x, y)) == Piece.WhiteMan)
                            possibleActions.AddRange(PossibleWhiteManActions(state, (x, y)));
                        else if (state.GetPieceAt((x, y)) == Piece.WhiteCrowned)
                            possibleActions.AddRange(PossibleWhiteCrownedActions(state, (x, y)));
                    }
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                for (int x = 0; x < BOARD_ROWS; x++)
                {
                    for (int y = 0; y < BOARD_COLS; y++)
                    {
                        if (state.GetPieceAt((x, y)) == Piece.BlackMan)
                            possibleActions.AddRange(PossibleBlackManActions(state, (x, y)));
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

        private IEnumerable<CheckersAction> PossibleBlackManActions(CheckersState state, (int x, int y) piecePosition)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CheckersAction> PossibleWhiteCrownedActions(CheckersState state, (int x, int y) piecePosition)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CheckersAction> PossibleWhiteManActions(CheckersState state, (int x, int y) start)
        {
            List<CheckersAction> possibleActions = new List<CheckersAction>();
            IEnumerable<List<(int, int, Piece)>> possibleCaptures = PossibleManCaptures(state, start);
            if (possibleCaptures.Count() > 0)
            {
                var sortedCaptures = possibleCaptures.OrderByDescending(capture => capture.Count());
                int maxLength = sortedCaptures.First().Count();
                var longestCaptures = sortedCaptures.All(capture => capture.Count() == maxLength);
                foreach ( var capture in sortedCaptures )
                {
                    possibleActions.Add(new CheckersAction(capture));
                }
                return possibleActions;
            }
            //no captures
            int x = start.x;
            int y = start.y;
            if (y < BOARD_ROWS - 1) //Doesn't check if turns into CrownedPiece in the process
            {
                if (x < BOARD_COLS - 1)
                    possibleActions.Add(new CheckersAction(
                        new List<(int, int, Piece)> { (x,y,Piece.None), (x + 1, y + 1,Piece.WhiteMan) }));
                if (x > 0)
                possibleActions.Add(new CheckersAction(
                    new List<(int, int, Piece)> { (x, y, Piece.None), (x - 1, y + 1, Piece.WhiteMan) }));
            }
            return possibleActions;
        }

        //Doesn't check if turns into CrownedPiece in the process
        private IEnumerable<List<(int, int, Piece)>> PossibleManCaptures(CheckersState state, (int x, int y) start)
        {
            List<List<(int, int, Piece)>> possibleCaptures = new List<List<(int, int, Piece)>>();
            IEnumerable<(int x, int y)> neighbours = GetNeighbours(start);
            Piece capturer = state.GetPieceAt(start);
            foreach ((int x, int y) neighbour in neighbours)
            {
                Piece target = state.GetPieceAt(neighbour);
                if (HaveOppositeColors(capturer, target))
                {
                    (int x, int y) vector = (neighbour.x - start.x, neighbour.y - start.y);
                    (int x, int y) endPosition = (neighbour.x + vector.x, neighbour.y + vector.y);
                    if (endPosition.x < BOARD_COLS && endPosition.x >= 0
                        && endPosition.y < BOARD_ROWS && endPosition.y >= 0
                        && state.GetPieceAt(endPosition) == Piece.None)
                    {
                        List<(int, int, Piece)> fieldsAfterCapture = new List<(int, int, Piece)> {
                            (start.x, start.y, Piece.None),
                            (neighbour.x, neighbour.y, Piece.None),
                            (endPosition.x, endPosition.y, capturer),
                        };
                        CheckersState newState = PerformAction(new CheckersAction(fieldsAfterCapture), state);
                        IEnumerable<List<(int, int, Piece)>> furtherCaptures = PossibleManCaptures(newState, endPosition);
                        List<List<(int, int, Piece)>> combinedCaptures = CombineCaptures((start.x, start.y, Piece.None),
                            (neighbour.x, neighbour.y, Piece.None), furtherCaptures);
                        if (combinedCaptures.Count() == 0) combinedCaptures.Add(fieldsAfterCapture);
                        possibleCaptures.AddRange(combinedCaptures);
                    }
                }
            }
            return possibleCaptures;
        }

        private List<List<(int, int, Piece)>> CombineCaptures((int, int, Piece) value1, (int, int, Piece) value2, IEnumerable<List<(int, int, Piece)>> furtherCaptures)
        {
            List<List<(int, int, Piece)>> combinedCaptures = new List<List<(int, int, Piece)>>();
            foreach (List<(int, int, Piece)> furtherCapture in furtherCaptures)
            {
                List<(int, int, Piece)> combinedCapture = new List<(int, int, Piece)> { value1, value2 };
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
            if (y < BOARD_ROWS - 1)
            {
                if (x < BOARD_COLS - 1)
                    neighbours.Add((x + 1, y + 1));
                if (x > 0)
                    neighbours.Add((x - 1, y + 1));
            }
            if (y > 0)
            {
                if (x < BOARD_COLS - 1)
                    neighbours.Add((x + 1, y - 1));
                if (x > 0)
                    neighbours.Add((x - 1, y - 1));
            }
            return neighbours;
        }

        private bool HaveOppositeColors(Piece capturer, Piece target)
        {
            if (capturer == Piece.None || target == Piece.None) return false;
            bool isCapturerWhite = capturer == Piece.WhiteMan || capturer == Piece.None;
            bool isTargetBlack = target == Piece.WhiteMan || target == Piece.None;
            return (isCapturerWhite && isTargetBlack) || (!isCapturerWhite && !isTargetBlack);
        }
    }
}