using Game.IGame;
using Gdk;
using Gtk;
using LanguageExt;
using static Game.Othello.OthelloState;

namespace Game.Othello
{
    public class Othello : IGame<OthelloAction, OthelloState, LanguageExt.Unit>
    {
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
            throw new NotImplementedException();
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
            while (i < 8)   // 8 do zmiany
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
            while(j < 8)
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
            List<OthelloAction> actions = new List<OthelloAction>();



            return actions;
        }
    }
}