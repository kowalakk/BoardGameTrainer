using Cairo;
using Game.IGame;
using LanguageExt;
using static Game.Othello.OthelloState;

namespace Game.Othello
{
    public partial class Othello : IGame<IOthelloAction, OthelloState, Unit>
    {
        public void DrawBoard(Context context, Unit u, OthelloState state, IEnumerable<(IOthelloAction, double)> ratedActions)
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

            context.Scale(1 / fieldSize, 1 / fieldSize);
        }

        private void DrawRatedAction(Context context, (IOthelloAction, double) ratedAction)
        {
            if (ratedAction.Item1 is OthelloEmptyAction)
            {
                context.SetSourceRGBA(0.9, 0.8, 0.6, 0.7);
                context.Rectangle(2, 3, 4, 2);
                context.Fill();

                context.SetSourceRGBA(0, 0, 0, 0.6);

                context.SelectFontFace("Sans", FontSlant.Normal, FontWeight.Normal);
                context.SetFontSize(0.34);
                context.MoveTo(2.05, 3.7);
                context.ShowText($"You have no legal moves!");
                context.MoveTo(2.8, 4.5);
                context.ShowText($"Click to skip turn");
                context.Stroke();

                return;
            }
            OthelloFullAction fullAction = (OthelloFullAction)ratedAction.Item1;
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
            if (field != Field.Empty)
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
    }
}