using Cairo;
using Game.IGame;
using LanguageExt;
using static Game.Othello.OthelloState;

namespace Game.Othello
{
    public partial class Othello : IGame<OthelloAction, OthelloState, Unit>
    {
        private const int boardSize = 8;
        private const double fieldSize = 1.0 / boardSize;

        public Player CurrentPlayer(OthelloState state)
        {
            return (state.BlacksTurn) ? Player.One : Player.Two;
        }

        public IEnumerable<OthelloAction> FilterByInputState(IEnumerable<OthelloAction> actions, Unit u)
        {
            return actions;
        }

        public GameResult Result(OthelloState state)
        {
            if (PossibleActions(state).Where(action => action is OthelloEmptyAction).Count() > 0)
                if(PossibleActions(new OthelloState(state.board, state.WhiteHandCount, state.BlackHandCount, !state.BlacksTurn)).Where(action => action is OthelloEmptyAction).Count() > 0)
                    return HasBlackWon(state);
            return GameResult.InProgress;
        }

        public OthelloState PerformAction(OthelloAction action, OthelloState state)
        {
            if (action.GetType() == typeof(OthelloEmptyAction))
                return new OthelloState(state.board, state.WhiteHandCount, state.BlackHandCount, !state.BlacksTurn);

            var playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            Field[,] board = new Field[boardSize, boardSize];
            Array.Copy(state.board, board, state.board.Length);
            OthelloFullAction fullAction = (OthelloFullAction)action;
            int x = fullAction.Position.Item1;
            int y = fullAction.Position.Item2;
            board[x, y] = fullAction.FieldContent;
            int i = x - 1;
            while(i >= x - fullAction.up)
            {
                board[i, y] = playersColor;
                i--;
            }
            i = x + 1;
            while (i <= x + fullAction.down)
            {
                board[i, y] = playersColor;
                i++;
            }
            int j = y - 1;
            while (j >= y - fullAction.left) 
            {
                board[x, j] = playersColor;
                j--;
            }
            j = y + 1;
            while (j <= y + fullAction.right)
            {
                board[x, j] = playersColor;
                j++;
            }
            i = x - 1;
            j = y - 1;
            while (i >= x - fullAction.upLeft && j >= y - fullAction.upLeft)
            {
                board[i, j] = playersColor;
                i--;
                j--;
            }
            i = x - 1;
            j = y + 1;
            while (i >= x - fullAction.upRight && j <= y + fullAction.upRight)
            {
                board[i, j] = playersColor;
                i--;
                j++;
            }
            i = x + 1;
            j = y - 1;
            while (i <= x + fullAction.downLeft && j >= y - fullAction.downLeft)
            {
                board[i, j] = playersColor;
                i++;
                j--;
            }
            i = x + 1;
            j = y + 1;
            while (i <= x + fullAction.downRight && j <= y + fullAction.downRight)
            {
                board[i, j] = playersColor;
                i++;
                j++;
            }

            int whiteCount = (state.BlacksTurn) ? state.WhiteHandCount : state.WhiteHandCount - 1;
            int blackCount = (state.BlacksTurn) ? state.BlackHandCount - 1 : state.BlackHandCount;
            return new OthelloState(board, whiteCount, blackCount, !state.BlacksTurn);
        }

        public IEnumerable<OthelloAction> PossibleActions(OthelloState state)
        {
            List<OthelloAction> actions = new List<OthelloAction>();
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
                        if (potentialAction.IsPotentialActionEmpty())
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
                return IGame.GameResult.Draw;
            return (blackCount > whiteCount) ? IGame.GameResult.PlayerOneWins : IGame.GameResult.PlayerTwoWins;
        }

        public (Unit, OthelloAction?) HandleInput(double x, double y, Unit inputState, OthelloState state)
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
            if(potentialAction.IsPotentialActionEmpty())
                return (new Unit(), null);
            Field playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            return (new Unit(), new OthelloFullAction((row, col), playersColor, potentialAction));
        }

        public IEnumerable<(OthelloAction, double)> FilterByInputState(IEnumerable<(OthelloAction, double)> ratedActions, Unit inputState, int numberOfActions)
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

        PotentialAction GetPotentialAction (int x, int y, OthelloState state)
        {
            int up = 0;
            int down = 0;
            int left = 0;
            int right = 0;
            int upLeft = 0;
            int upRight = 0;
            int downLeft = 0;
            int downRight = 0;
            Field playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            Field opponentsColor = (state.BlacksTurn) ? Field.White : Field.Black;

            // sprawdzenie w każdym z 8 kierunków czy sąsiaduje z szeregiem pionków przeciwnika, zakończonym własnym pionkiem     
            // up
            int oponentsPiecesCount = 0;
            for (int ii = x - 1; ii >= 0; ii--)
            {
                if (!ExploreDirection(ref oponentsPiecesCount, ref up, state, ii, y, opponentsColor, playersColor))
                    break;
            }

            // down
            oponentsPiecesCount = 0;
            for (int ii = x + 1; ii < boardSize; ii++)
            {
                if (!ExploreDirection(ref oponentsPiecesCount, ref down, state, ii, y, opponentsColor, playersColor))
                    break;
            }

            // left
            oponentsPiecesCount = 0;
            for (int jj = y - 1; jj >= 0; jj--)
            {
                if (!ExploreDirection(ref oponentsPiecesCount, ref left, state, x, jj, opponentsColor, playersColor))
                    break;
            }

            // right
            oponentsPiecesCount = 0;
            for (int jj = y + 1; jj < boardSize; jj++)
            {
                if (!ExploreDirection(ref oponentsPiecesCount, ref right, state, x, jj, opponentsColor, playersColor))
                    break;
            }

            // upLeft
            oponentsPiecesCount = 0;
            int i = x - 1;
            int j = y - 1;
            while(i >= 0 && j >= 0)
            {
                if (!ExploreDirection(ref oponentsPiecesCount, ref upLeft, state, i, j, opponentsColor, playersColor))
                    break;
                i--;
                j--;
            }

            // upRight
            oponentsPiecesCount = 0;
            i = x - 1;
            j = y + 1;
            while (i >= 0 && j < boardSize)
            {
                if (!ExploreDirection(ref oponentsPiecesCount, ref upRight, state, i, j, opponentsColor, playersColor))
                    break;
                i--;
                j++;
            }

            // downLeft
            oponentsPiecesCount = 0;
            i = x + 1;
            j = y - 1;
            while (i < boardSize && j >= 0)
            {
                if (!ExploreDirection(ref oponentsPiecesCount, ref downLeft, state, i, j, opponentsColor, playersColor))
                    break;
                i++;
                j--;
            }

            // downRight
            oponentsPiecesCount = 0;
            i = x + 1;
            j = y + 1;
            while (i < boardSize && j < boardSize)
            {
                if (!ExploreDirection(ref oponentsPiecesCount, ref downRight, state, i, j, opponentsColor, playersColor))
                    break;
                i++;
                j++;
            }
            return new PotentialAction(up, down, left, right, upLeft, upRight, downLeft, downRight);
        }

        private bool ExploreDirection(ref int oponentsPiecesCount, ref int direction, OthelloState state, int i, int j, OthelloState.Field opponentsColor, OthelloState.Field playersColor)
        {
            if (state.board[i, j] == Field.Empty)
                return false;
            if (state.board[i, j] == opponentsColor)
                oponentsPiecesCount++;
            if (state.board[i, j] == playersColor)
            {
                if (oponentsPiecesCount > 0)
                    direction = oponentsPiecesCount;
                return false;
            }
            return true;
        }
    }
}