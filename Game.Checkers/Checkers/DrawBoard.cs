using Cairo;
using Game.IGame;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, ICheckersInputState>
    {
        public void DrawBoard(Context context, ICheckersInputState inputState, CheckersState state, IEnumerable<(CheckersAction, double)> ratedActions)
        {
            double fieldSize = .125;
            context.SetSourceRGB(0.96, 0.85, 0.74);
            context.LineWidth = 0.001;
            context.Rectangle(0, 0, 1, 1);
            context.Fill();

            context.Scale(fieldSize, fieldSize);
            foreach (Field field in state.GetFields())
            {

                context.Translate(field.Col, (CheckersState.BOARD_SIZE - 1 - field.Row));
                DrawField(context);
                DrawPiece(context, state, field);
                context.Translate(-field.Col, -(CheckersState.BOARD_SIZE - 1 - field.Row));

            }
            context.Scale(1 / fieldSize, 1 / fieldSize);

        }

        private void DrawField(Context context)
        {
            context.SetSourceRGB(0.26, 0.13, 0);
            context.Rectangle(0, 0, 1, 1);
            context.Fill();
        }

        private void DrawPiece(Context context, CheckersState state, Field field)
        {
            Piece piece = state.GetPieceAt(field);
            if (piece == Piece.None)
                return;
            Color white = new(0.9, 0.9, 0.9);
            Color grey = new(0.5, 0.5, 0.5);
            Color black = new(0.1, 0.1, 0.1);
            if (piece == Piece.WhitePawn)
            {
                DrawPawn(context, white, grey);
                return;
            }
            if (piece == Piece.BlackPawn)
            {
                DrawPawn(context, black, grey);
                return;
            }
            if (piece == Piece.WhiteCrowned)
            {
                DrawCrowned(context, white, grey);
                return;
            }
            if (piece == Piece.BlackCrowned)
            {
                DrawCrowned(context, black, grey);
                return;
            }
        }

        private void DrawPawn(Context context, Color fill, Color border)
        {
            double lineWidth = context.LineWidth;
            context.LineWidth = 0.03;

            context.Arc(0.5, 0.5, 0.4, 0, 2 * Math.PI);
            context.SetSourceColor(fill);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.Arc(0.5, 0.5, 0.2, 0, 2 * Math.PI);
            context.Stroke();

            context.LineWidth = lineWidth;
        }

        private void DrawCrowned(Context context, Color fill, Color border)
        {
            double lineWidth = context.LineWidth;
            context.LineWidth = 0.03;
            context.Arc(0.5, 0.5, 0.4, 0, 2 * Math.PI);
            context.SetSourceColor(fill);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.Arc(0.5, 0.5, 0.2, 0, 2 * Math.PI);
            context.SetSourceRGB(0.831, 0.686, 0.216);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.LineWidth = lineWidth;
        }
    }
}
