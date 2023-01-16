﻿using Cairo;
using Game.IGame;
using Gtk;

namespace Game.Checkers
{
    public partial class Checkers : IGame<CheckersAction, CheckersState, ICheckersInputState>
    {
        private readonly double FIELD_SIZE = .125;
        public void DrawBoard(Context context, ICheckersInputState inputState, CheckersState state, IEnumerable<(CheckersAction, double)> ratedActions)
        {
            context.SetSourceRGB(0.96, 0.85, 0.74);
            context.LineWidth = 0.001;
            context.Rectangle(0, 0, 1, 1);
            context.Fill();

            context.Scale(FIELD_SIZE, FIELD_SIZE);
            foreach (Field field in state.GetFields())
            {

                context.Translate(field.Col, (CheckersState.BOARD_SIZE - 1 - field.Row));
                DrawField(context);
                context.Translate(-field.Col, -(CheckersState.BOARD_SIZE - 1 - field.Row));

            }
            context.Scale(1 / FIELD_SIZE, 1 / FIELD_SIZE);
            DrawGameState(context, inputState, ratedActions);
            context.Scale(FIELD_SIZE, FIELD_SIZE);
            foreach (Field field in state.GetFields())
            {

                context.Translate(field.Col, (CheckersState.BOARD_SIZE - 1 - field.Row));
                DrawPiece(context, state, field);
                context.Translate(-field.Col, -(CheckersState.BOARD_SIZE - 1 - field.Row));

            }
            context.Scale(1 / FIELD_SIZE, 1 / FIELD_SIZE);

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

            context.Arc(0.5, 0.5, 0.3, 0, 2 * Math.PI);
            context.SetSourceColor(fill);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.Arc(0.5, 0.5, 0.15, 0, 2 * Math.PI);
            context.Stroke();

            context.LineWidth = lineWidth;
        }

        private void DrawCrowned(Context context, Color fill, Color border)
        {
            double lineWidth = context.LineWidth;
            context.LineWidth = 0.03;
            context.Arc(0.5, 0.5, 0.3, 0, 2 * Math.PI);
            context.SetSourceColor(fill);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.Arc(0.5, 0.5, 0.15, 0, 2 * Math.PI);
            context.SetSourceRGB(0.831, 0.686, 0.216);
            context.FillPreserve();

            context.SetSourceColor(border);
            context.Stroke();

            context.LineWidth = lineWidth;
        }

        private void DrawGameState(Context context, ICheckersInputState inputState, IEnumerable<(CheckersAction, double)> ratedActions)
        {
            if (inputState is IdleCIS) // draw best actions
            {
                Color green = new(0.5, 1, 0);
                foreach (var action in ratedActions)
                {
                    DrawRatedAction(context, action, green);
                }
                return;
            }
            if (inputState is MarkedPieceCIS markedPieceState) // draw actions for marked piece
            {
                Color blue = new(0, 0.75, 1);
                foreach (var action in ratedActions)
                {
                    DrawRatedAction(context, action, blue);
                }
                DrawMarkedField(context, markedPieceState.MarkedField);
                return;
            }
            { // draw actions for ongoing action
                Color blue = new(0, 0.75, 1);
                CaptureActionInProgressCIS actionInProgressState = (CaptureActionInProgressCIS)inputState;
                foreach (var action in ratedActions)
                {
                    DrawRatedAction(context, action, blue);
                }

                DrawVisitedFields(context, actionInProgressState.VisitedFields);
                return;
            }
        }

        private void DrawMarkedField(Context context, Field field)
        {
            Color orange = new(1, 0.75, 0);
            context.Scale(FIELD_SIZE, FIELD_SIZE);
            context.Translate(field.Col, (CheckersState.BOARD_SIZE - 1 - field.Row));
            DrawSpecialField(context, orange);
            context.Translate(-field.Col, -(CheckersState.BOARD_SIZE - 1 - field.Row));
            context.Scale(1 / FIELD_SIZE, 1 / FIELD_SIZE);
        }

        private void DrawRatedAction(Context context, (CheckersAction, double) action, Color color)
        {
            IEnumerable<Field> fields = action.Item1.GetClickableFields();
            
            context.Scale(FIELD_SIZE, FIELD_SIZE);
            foreach (Field field in fields)
            {
                context.Translate(field.Col, (CheckersState.BOARD_SIZE - 1 - field.Row));
                DrawSpecialField(context, color);
                context.Translate(-field.Col, -(CheckersState.BOARD_SIZE - 1 - field.Row));
            }
            Field lastField = fields.Last();
            int rating = (int)((action.Item2 + 1) * 50);

            context.Translate(lastField.Col, (CheckersState.BOARD_SIZE - 1 - lastField.Row));
            DrawRating(context, rating);
            context.Translate(-lastField.Col, -(CheckersState.BOARD_SIZE - 1 - lastField.Row));
            context.Scale(1 / FIELD_SIZE, 1 / FIELD_SIZE);
        }

        private void DrawVisitedFields(Context context, IEnumerable<Field> fields)
        {
            Color red = new(1, 0.34, 0.2);
            context.Scale(FIELD_SIZE, FIELD_SIZE);
            foreach (Field field in fields)
            {
                context.Translate(field.Col, (CheckersState.BOARD_SIZE - 1 - field.Row));
                DrawSpecialField(context, red);
                context.Translate(-field.Col, -(CheckersState.BOARD_SIZE - 1 - field.Row));
            }
            context.Scale(1 / FIELD_SIZE, 1 / FIELD_SIZE);
        }

        private void DrawSpecialField(Context context, Color fill)
        {
            //double lineWidth = context.LineWidth;
            //context.LineWidth = 0.2;

            //context.Rectangle(0.1, 0.1, 0.8, 0.8);
            context.SetSourceColor(fill);
            //context.Stroke();

            //context.LineWidth = lineWidth;
            context.Rectangle(0, 0, 1, 1);
            context.Fill();
        }

        private void DrawRating(Context context, int rating)
        {
            context.SetSourceRGB(1, 1, 1);
            context.SelectFontFace("Sans", FontSlant.Normal, FontWeight.Normal);
            context.SetFontSize(0.2);
            context.MoveTo(0, 0.2);
            context.ShowText($"{rating}%");
            context.Stroke();
        }
    }
}
