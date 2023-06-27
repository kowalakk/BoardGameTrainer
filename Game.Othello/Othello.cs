using Game.IGame;
using LanguageExt;
using static Game.Othello.OthelloState;

namespace Game.Othello
{
    public partial class Othello : IGame<IOthelloAction, OthelloState, Unit>
    {
        private const int boardSize = 8;
        private const double fieldSize = 1.0 / boardSize;

        private static List<(int dcol, int drow, Func<int, int, int> fields)> directions = new()
        {
            ( 0, -1, (int col, int row) => row), // up
            ( 0,  1, (int col, int row) => boardSize - 1 - row), // down
            (-1,  0, (int col, int row) => col), // left
            ( 1,  0, (int col, int row) => boardSize - 1 - col), // right
            (-1, -1, Math.Min), // upLeft
            ( 1, -1, (int col, int row) => Math.Min(row, boardSize - 1 - col)), // upRight
            (-1,  1, (int col, int row) => Math.Min(boardSize - 1 - row, col)), // downLeft
            ( 1,  1, (int col, int row) => Math.Min(boardSize - 1 - row, boardSize - 1 - col)), // downRight
        };

        public Player CurrentPlayer(OthelloState state)
        {
            return (state.BlacksTurn) ? Player.One : Player.Two;
        }

        public IEnumerable<IOthelloAction> FilterByInputState(IEnumerable<IOthelloAction> actions, Unit u)
        {
            return actions;
        }

        public GameResult Result(OthelloState state)
        {
            if (PossibleActions(state).Any(action => action is OthelloEmptyAction))
                if (PossibleActions(new OthelloState(state.board, state.WhiteHandCount, state.BlackHandCount, !state.BlacksTurn)).Any(action => action is OthelloEmptyAction))
                    return HasBlackWon(state);
            return GameResult.InProgress;
        }

        public OthelloState PerformAction(IOthelloAction action, OthelloState state)
        {
            if (action is OthelloEmptyAction)
                return new OthelloState(state.board, state.WhiteHandCount, state.BlackHandCount, !state.BlacksTurn);

            var playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            Field[,] board = new Field[boardSize, boardSize];
            Array.Copy(state.board, board, state.board.Length);
            OthelloFullAction fullAction = (OthelloFullAction)action;
            int x = fullAction.Position.Item1;
            int y = fullAction.Position.Item2;
            board[x, y] = fullAction.FieldContent;

            for (int i = 0; i < 8; i++)
            {
                var (dCol, dRow, _) = directions[i];
                int col = x;
                int row = y;
                for (int j = 0; j < fullAction.PiecesToFlip[i]; j++)
                {
                    col += dCol;
                    row += dRow;
                    board[col, row] = playersColor;
                }
            }

            int whiteCount = (state.BlacksTurn) ? state.WhiteHandCount : state.WhiteHandCount - 1;
            int blackCount = (state.BlacksTurn) ? state.BlackHandCount - 1 : state.BlackHandCount;
            return new OthelloState(board, whiteCount, blackCount, !state.BlacksTurn);
        }

        public IEnumerable<IOthelloAction> PossibleActions(OthelloState state)
        {
            List<IOthelloAction> actions = new List<IOthelloAction>();
            if ((state.BlacksTurn && state.BlackHandCount == 0) || (!state.BlacksTurn && state.WhiteHandCount == 0))
            {
                actions.Add(new OthelloEmptyAction());
                return actions;
            }
            Field playersColor = (state.BlacksTurn) ? Field.Black : Field.White;

            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    if (state.board[i, j] == Field.Empty)
                    {
                        PotentialAction potentialAction = GetPotentialAction(i, j, state);
                        if (potentialAction.IsEmpty())
                            continue;
                        actions.Add(new OthelloFullAction((i, j), playersColor, potentialAction));
                    }
            if (!actions.Any())
                actions.Add(new OthelloEmptyAction());

            return actions;
        }

        private GameResult HasBlackWon(OthelloState state)
        {
            int whiteCount = 0;
            int blackCount = 0;
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                {
                    if (state.board[i, j] == Field.White)
                        whiteCount++;
                    if (state.board[i, j] == Field.Black)
                        blackCount++;
                }
            if (whiteCount == blackCount)
                return GameResult.Draw;
            return (blackCount > whiteCount) ? GameResult.PlayerOneWins : GameResult.PlayerTwoWins;
        }

        public (Unit, IOthelloAction?) HandleInput(double x, double y, Unit inputState, OthelloState state)
        {
            if (PossibleActions(state).First() is OthelloEmptyAction action)
                return (new Unit(), action);
            if (x < 0 || x > 1 || y < 0 || y > 1)
                return (new Unit(), null);
            int col = (int)(x * boardSize);
            int row = (int)(y * boardSize);
            if (state.board[row, col] != Field.Empty)
                return (new Unit(), null);
            PotentialAction potentialAction = GetPotentialAction(row, col, state);
            if (potentialAction.IsEmpty())
                return (new Unit(), null);
            Field playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            return (new Unit(), new OthelloFullAction((row, col), playersColor, potentialAction));
        }

        public IEnumerable<(IOthelloAction, double)> FilterByInputState(IEnumerable<(IOthelloAction, double)> ratedActions, Unit inputState, int numberOfActions)
        {
            return ratedActions.Take(numberOfActions);
        }

        public Unit EmptyInputState()
        {
            return new Unit();
        }

        public OthelloState InitialState()
        {
            return GenerateInitialOthelloState();
        }

        PotentialAction GetPotentialAction(int x, int y, OthelloState state)
        {
            int[] flippedPieces = new int[8];
            (Field playersColor, Field opponentsColor) =
                (state.BlacksTurn) ? (Field.Black, Field.White) : (Field.White, Field.Black);

            // sprawdzenie w każdym z 8 kierunków czy sąsiaduje z szeregiem pionków przeciwnika, zakończonym własnym pionkiem     
            for (int i = 0; i < 8; i++)
            {
                var (dcol, drow, fields) = directions[i];
                int col = x + dcol;
                int row = y + drow;
                int fieldsToCheck = fields(x, y) - 1;
                int opponentsPieces = 0;
                while (opponentsPieces < fieldsToCheck && state.board[col, row] == opponentsColor)
                {
                    opponentsPieces++;
                    col += dcol;
                    row += drow;
                }
                if (opponentsPieces <= fieldsToCheck && state.board[col, row] == playersColor)
                {
                    flippedPieces[i] = opponentsPieces;
                }
            }
            return new PotentialAction(flippedPieces);
        }
    }
}