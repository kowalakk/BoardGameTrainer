using Cairo;
using Game.IGame;
using Gdk;
using Gtk;
using LanguageExt;
using static Game.Othello.OthelloState;

namespace Game.Othello
{
    public class Othello : IGame<OthelloAction, OthelloState, LanguageExt.Unit>
    {
        private const int boardSize = 8;

        public Player CurrentPlayer(OthelloState state)
        {
            return (state.BlacksTurn) ? Player.One : Player.Two;
        }

        public void DrawBoard(Context context, LanguageExt.Unit u, OthelloState state, IEnumerable<(OthelloAction, double)> ratedActions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OthelloAction> FilterByInputState(IEnumerable<OthelloAction> actions, LanguageExt.Unit u)
        {
            return actions;
        }

        public GameResult Result(OthelloState state)
        {
            if (PossibleActions(state).Where(action => action is OthelloEmptyAction).Count() > 0)
                if(PossibleActions(new OthelloState(state.board, state.WhiteHandCount, state.BlackHandCount, !state.BlacksTurn)).Where(action => action is OthelloEmptyAction).Count() > 0)
                    return HasBlackWon(state);
            return IGame.GameResult.InProgress;
        }

        public OthelloState PerformAction(OthelloAction action, OthelloState state)
        {
            if (action.GetType() == typeof(OthelloEmptyAction))
                return new OthelloState(state.board, state.WhiteHandCount, state.BlackHandCount, !state.BlacksTurn);

            var playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            var oponentsColor = (state.BlacksTurn) ? Field.White : Field.Black;
            Field[,] board = state.board;
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
                board[j, y] = playersColor;
                j--;
            }
            j = y + 1;
            while (j <= y + fullAction.right)
            {
                board[j, y] = playersColor;
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
            Field opponentsColor = (state.BlacksTurn) ? Field.White : Field.Black;

            (int, int, int, int) GetPotentialAction(int x, int y)
            {
                int up = 0;
                int down = 0;
                int left = 0;
                int right = 0;
                // sprawdzenie w każdym z 4 kierunków czy sąsiaduje z szeregiem pionków przeciwnika, zakończonym własnym pionkiem
                int oponentsPiecesCount = 0;
                for(int i = x - 1; i >= 0; i--)
                {
                    if (state.board[i, y] == Field.Empty)
                        break;
                    if (state.board[i, y] == opponentsColor)
                        oponentsPiecesCount++;
                    if (state.board[i, y] == playersColor)
                    {
                        if (oponentsPiecesCount > 0)
                            up = oponentsPiecesCount;
                        break;
                    }
                    
                }
                oponentsPiecesCount = 0;
                
                for (int i = x + 1; i < boardSize; i++)
                {
                    if (state.board[i, y] == Field.Empty)
                        break;
                    if (state.board[i, y] == opponentsColor)
                        oponentsPiecesCount++;
                    if (state.board[i, y] == playersColor)
                    {
                        if (oponentsPiecesCount > 0)
                            down = oponentsPiecesCount;
                        break;
                    }
                }
                oponentsPiecesCount = 0;
                for (int j = y - 1; j >= 0; j--)
                {
                    if (state.board[x, j] == Field.Empty)
                        break;
                    if (state.board[x, j] == opponentsColor)
                        oponentsPiecesCount++;
                    if (state.board[x, j] == playersColor)
                    {
                        if (oponentsPiecesCount > 0)
                            left = oponentsPiecesCount;
                        break;
                    }
                }
                oponentsPiecesCount = 0;
                for (int j = y + 1;  j < boardSize; j++)
                {
                    if (state.board[x, j] == Field.Empty)
                        break;
                    if (state.board[x, j] == opponentsColor)
                        oponentsPiecesCount++;
                    if (state.board[x, j] == playersColor)
                    {
                        if (oponentsPiecesCount > 0)
                            right = oponentsPiecesCount;
                        break;
                    }
                }
                return (up, down, left, right);
            }    
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    if (state.board[i, j] == Field.Empty)
                    {
                        (int up, int down, int left, int right) potentialAction = GetPotentialAction(i, j);
                        if (potentialAction.up == 0 && potentialAction.down == 0 && potentialAction.left == 0 && potentialAction.right == 0)
                            continue;
                        actions.Add(new OthelloFullAction((i, j), playersColor, potentialAction.up, potentialAction.down, potentialAction.left, potentialAction.right));
                    }
                        
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

        public (LanguageExt.Unit, OthelloAction?) HandleInput(double x, double y, LanguageExt.Unit inputState, OthelloState state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<(OthelloAction, double)> FilterByInputState(IEnumerable<(OthelloAction, double)> ratedActions, LanguageExt.Unit inputState, int bestShownActionsCount)
        {
            return ratedActions;
        }

        public LanguageExt.Unit EmptyInputState()
        {
            return LanguageExt.Unit.Default;
        }

        public OthelloState InitialState()
        {
            return OthelloState.GenerateInitialOthelloState();
        }
    }
}