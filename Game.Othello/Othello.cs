using Cairo;
using Game.IGame;
using LanguageExt;
using static Game.Othello.OthelloState;

namespace Game.Othello
{
    public class Othello : IGame<OthelloAction, OthelloState, Unit>
    {
        private const int boardSize = 8;
        private const double fieldSize = 1.0 / boardSize;

        public Player CurrentPlayer(OthelloState state)
        {
            return (state.BlacksTurn) ? Player.One : Player.Two;
        }

        public void DrawBoard(Context context, Unit u, OthelloState state, IEnumerable<(OthelloAction, double)> ratedActions)
        {
            context.SetSourceRGB(0.86, 0.85, 0.74);
            context.LineWidth = 0.001;
            context.Rectangle(0, 0, 1, 1);
            context.Fill();
            context.SetSourceRGB(0, 0, 0);
            
            for (double position = 0; position <= 1; position += 0.125)
            {
                context.MoveTo(position, 0);
                context.LineTo(position, 1);
                context.Stroke();
                context.MoveTo(0, position);
                context.LineTo(1, position);
                context.Stroke();
            }
            context.Scale(fieldSize, fieldSize);

            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                {
                    context.Translate(j, i);
                    DrawPiece(context, state.board[i, j]);
                    context.Translate(-j, -i);
                }

            foreach (var action in ratedActions)
            {
                DrawRatedAction(context, action);
            }

            context.Scale(1 / fieldSize, 1/ fieldSize);
        }

        private void DrawRatedAction(Context context, (OthelloAction, double) ratedAction)
        {
            if (ratedAction.Item1 is OthelloEmptyAction)
            {
                context.SetSourceRGBA(0.9, 0.8, 0.6, 0.7);
                context.Rectangle(2, 3, 4, 2);
                context.Fill();

                context.SetSourceRGBA(0,0,0, 0.6);

                context.SelectFontFace("Sans", FontSlant.Normal, FontWeight.Normal);
                context.SetFontSize(0.34);
                context.MoveTo(2.05, 3.7);
                context.ShowText($"You have no legal moves!");
                context.MoveTo(2.8, 4.5);
                context.ShowText($"Click to skip turn");
                context.Stroke();

                return;
            }
            OthelloFullAction fullAction = (OthelloFullAction) ratedAction.Item1;
            int rating = (int)((ratedAction.Item2 + 1) * 50);
            context.Translate(fullAction.Position.Item2, fullAction.Position.Item1);
            context.SetSourceRGB(0.4, 0.5 + ratedAction.Item2 / 2, 0);
            context.Rectangle(0, 0, 1, 1);
            context.Fill();
            context.SetSourceRGB(0, 0, 0);
            context.SelectFontFace("Sans", FontSlant.Normal, FontWeight.Normal);
            context.SetFontSize(0.2);
            context.MoveTo(0, 0.2);
            context.ShowText($"{rating}%");
            context.Stroke();
            context.Translate(-fullAction.Position.Item2, -fullAction.Position.Item1);
        }

        private void DrawPiece(Context context, Field field)
        {
            if(field != Field.Empty)
            {
                Color white = new(0.9, 0.9, 0.9);
                Color grey = new(0.5, 0.5, 0.5);
                Color black = new(0.1, 0.1, 0.1);

                Color color = (field == Field.Black) ? black : white;
                double lineWidth = context.LineWidth;
                context.LineWidth = 0.03;

                context.Arc(0.5, 0.5, 0.4, 0, 2 * Math.PI);
                context.SetSourceColor(color);
                context.FillPreserve();

                context.SetSourceColor(grey);
                context.Stroke();

                context.LineWidth = lineWidth;
            }
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
                        (int up, int down, int left, int right) potentialAction = GetPotentialAction(i, j, state);
                        if (potentialAction.up == 0 && potentialAction.down == 0 && potentialAction.left == 0 && potentialAction.right == 0)
                            continue;
                        actions.Add(new OthelloFullAction((i, j), playersColor, potentialAction.up, potentialAction.down, potentialAction.left, potentialAction.right));
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
            (int up, int down, int left, int right) potentialAction = GetPotentialAction(row, col, state);
            if(potentialAction.up == 0 && potentialAction.down == 0 && potentialAction.left == 0 && potentialAction.right == 0)
                return (new Unit(), null);
            Field playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            return (new Unit(), new OthelloFullAction((row, col), playersColor, potentialAction.up, potentialAction.down, potentialAction.left, potentialAction.right));
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

        (int up, int down, int left, int right) GetPotentialAction(int x, int y, OthelloState state)
        {
            int up = 0;
            int down = 0;
            int left = 0;
            int right = 0;
            Field playersColor = (state.BlacksTurn) ? Field.Black : Field.White;
            Field opponentsColor = (state.BlacksTurn) ? Field.White : Field.Black;

            // sprawdzenie w każdym z 4 kierunków czy sąsiaduje z szeregiem pionków przeciwnika, zakończonym własnym pionkiem
            int oponentsPiecesCount = 0;
            for (int i = x - 1; i >= 0; i--)
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
            for (int j = y + 1; j < boardSize; j++)
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
    }
}