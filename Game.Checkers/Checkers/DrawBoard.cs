using Cairo;
using Game.IGame;
using Gtk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, ICheckersInputState>
    {
        private static readonly double fieldSize = 0.125;
        private static readonly Color white = new(0.9, 0.9, 0.9);
        private static readonly Color grey = new(0.5, 0.5, 0.5);
        private static readonly Color black = new(0.1, 0.1, 0.1);
        private static readonly Color gold = new(0.831, 0.686, 0.216);
        private static readonly Color blue = new(0, 0.5, 0.75);
        private static readonly Color green = new(0.5, 1, 0);
        private static readonly Color red = new(0.8, 0.4, 0.0);
        //private static readonly Color red = new(0.8, 0.3, 0.3);
        private static readonly Color brown = new(0.96, 0.85, 0.74);
        private static readonly Color beige = new(0.26, 0.13, 0);
        private Field lastClickedField;

        public void DrawBoard(Context context, ICheckersInputState inputState, CheckersState state, IEnumerable<(CheckersAction, double)> ratedActions)
        {
            lastClickedField = new(0, 7);
            context.SetSourceColor(brown);
            context.LineWidth = 0.001;
            context.Rectangle(0, 0, 1, 1);
            context.Fill();

            foreach (Field field in state.GetFields())
            {
                MoveContextToField(context, field);
                DrawField(context);
                DrawPiece(context, state, field);
            }
            DrawGameState(context, inputState, state, ratedActions);
        }
        private void MoveContextToField(Context context, Field field)
        {
            context.Translate(
                (field.Col - lastClickedField.Col) * fieldSize,
                (lastClickedField.Row - field.Row) * fieldSize);
            lastClickedField = field;
        }

        private void DrawField(Context context)
        {
            context.SetSourceColor(beige);
            context.Rectangle(0, 0, fieldSize, fieldSize);
            context.Fill();
        }

        private void DrawPiece(Context context, CheckersState state, Field field)
        {
            Piece piece = state.GetPieceAt(field);
            if (piece == Piece.None)
                return;
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
            context.LineWidth = 0.03 * fieldSize;

            context.Arc(0.5 * fieldSize, 0.5 * fieldSize, 0.3 * fieldSize, 0, 2 * Math.PI);
            context.SetSourceColor(fill);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.Arc(0.5 * fieldSize, 0.5 * fieldSize, 0.15 * fieldSize, 0, 2 * Math.PI);
            context.Stroke();

            context.LineWidth = lineWidth;
        }

        private void DrawCrowned(Context context, Color fill, Color border)
        {
            double lineWidth = context.LineWidth;
            context.LineWidth = 0.03 * fieldSize;
            context.Arc(0.5 * fieldSize, 0.5 * fieldSize, 0.3 * fieldSize, 0, 2 * Math.PI);
            context.SetSourceColor(fill);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.Arc(0.5 * fieldSize, 0.5 * fieldSize, 0.15 * fieldSize, 0, 2 * Math.PI);
            context.SetSourceColor(gold);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.LineWidth = lineWidth;
        }

        private void DrawGameState(Context context, ICheckersInputState inputState, CheckersState state, IEnumerable<(CheckersAction, double)> ratedActions)
        {
            if (inputState is DefaultInputState) // draw best actions
            {
                foreach (var action in ratedActions.Reverse())
                {
                    DrawRatedAction(context, state, action, green);
                }
                return;
            }
            if (inputState is MarkedPieceInputState markedPieceState) // draw actions for marked piece
            {
                foreach (var action in ratedActions)
                {
                    DrawRatedAction(context, state, action, blue);
                }
                DrawMarkedField(context, state, markedPieceState.MarkedField);
                return;
            }
            // draw actions for ongoing action
            CaptureActionInProgressInputState actionInProgressState = (CaptureActionInProgressInputState)inputState;
            foreach (var action in ratedActions)
            {
                DrawRatedAction(context, state, action, blue);
            }

            DrawVisitedFields(context, actionInProgressState.VisitedFields);
            Field markedField = actionInProgressState.VisitedFields.Last();
            DrawMarkedField(context, state, markedField);
            DrawPiece(context, state, actionInProgressState.VisitedFields.First());

        }

        private void DrawMarkedField(Context context, CheckersState state, Field field)
        {
            MoveContextToField(context, field);
            DrawSpecialField(context, blue);
            DrawPiece(context, state, field);
        }

        private void DrawRatedAction(Context context, CheckersState state, (CheckersAction, double) action, Color color)
        {
            IEnumerable<Field> fields = action.Item1.GetParticipatingFields();
            Color colorFromRating = new((1-action.Item2)*0.75, (1 + action.Item2) * 0.75, 0);

            foreach (Field field in fields)
            {
                MoveContextToField(context, field);
                DrawSpecialField(context, colorFromRating);
                DrawPiece(context, state, field);
            }
            int rating = (int)((action.Item2 + 1) * 50);
            DrawRating(context, rating);

            if (action.Item1 is CaptureAction captureAction)
            {
                foreach(Field field in captureAction.GetCapturedFields())
                {
                    MoveContextToField(context, field);
                    DrawSpecialField(context, red);
                    DrawPiece(context, state, field);
                }
            }
        }

        private void DrawVisitedFields(Context context, IEnumerable<Field> fields)
        {
            foreach (Field field in fields)
            {
                MoveContextToField(context, field);
                DrawSpecialField(context, blue);
            }
        }

        private void DrawSpecialField(Context context, Color fill)
        {
            context.SetSourceColor(fill);
            context.Rectangle(0, 0, fieldSize, fieldSize);
            context.Fill();
        }

        private void DrawRating(Context context, int rating)
        {
            context.SetSourceRGB(0, 0, 0);
            context.SelectFontFace("Sans", FontSlant.Normal, FontWeight.Normal);
            context.SetFontSize(0.2 * fieldSize);
            context.MoveTo(0 * fieldSize, 0.2 * fieldSize);
            context.ShowText($"{rating}%");
            context.Stroke();
        }
    }
}
