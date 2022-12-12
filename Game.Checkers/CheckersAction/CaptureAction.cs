namespace Game.Checkers
{
    public struct SimpleCapture
    {
        public Field Captured { get; private set; }
        public Field Finish { get; private set; }

        public SimpleCapture(Field captured, Field finish)
        {
            Captured = captured;
            Finish = finish;
        }
    }

    public class CaptureAction : CheckersAction
    {
        public override Field Finish { get => Captures.Last!.Value.Finish; }

        public LinkedList<SimpleCapture> Captures { get; private set; }
        public int CapturesCount { get; private set; }

        public CaptureAction(Field start, Field capture, Field finish) : base(start)
        {
            CapturesCount = 1;
            Captures = new LinkedList<SimpleCapture>();
            Captures.AddFirst(new SimpleCapture(capture, finish));
        }
        public void CombineCapture(Field start, Field firstCapture)
        {
            SimpleCapture simpleCapture = new(firstCapture, Start);
            Captures.AddFirst(simpleCapture);
            Start = start;
            CapturesCount++;
        }

        public override bool Equals(CheckersAction? other)
        {
            if (other == null) return false;
            if (other is not CaptureAction) return false;
            CaptureAction moveAction = (CaptureAction)other;
            if (!moveAction.Start.Equals(Start)) return false;
            if (!moveAction.Captures.SequenceEqual(Captures)) return false;
            return true;
        }

        public override CheckersState PerformOn(CheckersState state)
        {
            return PerformOn(state);
        }

        public CheckersState PerformOn(CheckersState state, Piece substituteCapturedWith = Piece.None)
        {
            Piece capturer = state.GetPieceAt(Start);
            CheckersState newState = new(state);
            newState.SetPieceAt(Start, Piece.None);
            foreach (SimpleCapture capture in Captures)
            {
                newState.SetPieceAt(capture.Captured, substituteCapturedWith);
            }
            newState.SetPieceAtWithPossiblePromotion(Finish, capturer);
            return newState;
        }
    }
}

