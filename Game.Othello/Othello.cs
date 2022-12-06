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
        public void DrawBoard(Widget widget, LanguageExt.Unit u, OthelloState state, IEnumerable<(OthelloAction, double)> ratedActions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OthelloAction> FilterByInputState(IEnumerable<OthelloAction> actions, LanguageExt.Unit u)
        {
            return actions;
        }

        public GameResults GameResult(OthelloState state)
        {
            if(PossibleActions(state) == null)
            {
                if(PossibleActions(new OthelloState(state.board, state.WhiteHandCount, state.BlackHandCount, !state.BlacksTurn)) == null)
                {
                    int hasBlackWon = HasBlackWon(state);
                    if (hasBlackWon == 0)
                        return GameResults.Draw;
                    return (hasBlackWon == 1) ? GameResults.PlayerOneWins : GameResults.PlayerTwoWins;
                }
            }
            return GameResults.InProgress;
        }

        public (OthelloAction, LanguageExt.Unit) HandleInput(Event @event, LanguageExt.Unit u, OthelloState state)
        {
            throw new NotImplementedException();
        }

        public OthelloState PerformAction(OthelloAction action, OthelloState state)
        {
            if (action == null)
                return new OthelloState(state.board, state.WhiteHandCount, state.BlackHandCount, !state.BlacksTurn);

            var playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            var oponentsColor = (state.BlacksTurn) ? Field.White : Field.Black;
            Field[,] board = state.board;
            int x = action.Position.Item1;
            int y = action.Position.Item2;
            board[x, y] = action.FieldContent;
            int i = x--;
            while(i > 0)
            {
                if (board[i, y] == oponentsColor)
                {
                    board[i, y] = playersColor;
                    i--;
                }
                else
                    break;
            }
            i = x++;
            while (i < boardSize)
            {
                if (board[i, y] == oponentsColor)
                {
                    board[i, y] = playersColor;
                    i++;
                }
                else
                    break;
            }
            int j = y--;
            while(j > 0) 
            {
                if (board[x, j] == oponentsColor)
                {
                    board[x, j] = playersColor;
                    j--;
                }
                else
                    break;
            }
            j = y++;
            while(j < boardSize)
            {
                if (board[x, j] == oponentsColor)
                {
                    board[x, j] = playersColor;
                    j++;
                }
                else
                    break;
            }
            int whiteCount = (state.BlacksTurn) ? state.WhiteHandCount : state.WhiteHandCount - 1;
            int blackCount = (state.BlacksTurn) ? state.BlackHandCount - 1 : state.BlackHandCount;
            return new OthelloState(board, whiteCount, blackCount, !state.BlacksTurn);
        }

        public IEnumerable<OthelloAction> PossibleActions(OthelloState state)
        {
            Field playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            Field opponentsColor = (state.BlacksTurn) ? Field.White : Field.Black;

            bool CheckIfActionPossible(int x, int y)
            {
                int i = x - 1;
                int j = y - 1;
                int oponentsPiecesCount = 0;
                while(i >= 0)
                {
                    if (state.board[i, y] == Field.Empty)
                        break;
                    if (state.board[i, y] == opponentsColor)
                        oponentsPiecesCount++;
                    if (state.board[i, y] == playersColor)
                    {
                        if (oponentsPiecesCount > 0)
                            return true;
                        break;
                    }
                    i--;
                }
                oponentsPiecesCount = 0;
                while (i < boardSize)
                {
                    if (state.board[i, y] == Field.Empty)
                        break;
                    if (state.board[i, y] == opponentsColor)
                        oponentsPiecesCount++;
                    if (state.board[i, y] == playersColor)
                    {
                        if (oponentsPiecesCount > 0)
                            return true;
                        break;
                    }
                    i++;
                }
                oponentsPiecesCount = 0;
                while (j >= 0)
                {
                    if (state.board[x, j] == Field.Empty)
                        break;
                    if (state.board[x, j] == opponentsColor)
                        oponentsPiecesCount++;
                    if (state.board[x, j] == playersColor)
                    {
                        if (oponentsPiecesCount > 0)
                            return true;
                        break;
                    }
                    j--;
                }
                oponentsPiecesCount = 0;
                while (j < boardSize)
                {
                    if (state.board[x, j] == Field.Empty)
                        break;
                    if (state.board[x, j] == opponentsColor)
                        oponentsPiecesCount++;
                    if (state.board[x, j] == playersColor)
                    {
                        if (oponentsPiecesCount > 0)
                            return true;
                        break;
                    }
                    j++;
                }
                return false;
            }
            List<OthelloAction> actions = new List<OthelloAction>();
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    if (state.board[i, j] == Field.Empty)
                        if (CheckIfActionPossible(i, j))
                            actions.Add(new OthelloAction((i, j), playersColor));
            return actions;
        }



        private int HasBlackWon(OthelloState state)
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
                return 0;
            return (blackCount > whiteCount) ? 1 : -1;
        }
    }
}