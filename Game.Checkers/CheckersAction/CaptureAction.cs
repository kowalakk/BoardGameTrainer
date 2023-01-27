using Game.IGame;

namespace Game.Checkers
{
    public struct SimpleCapture
    {
        public Field Start { get; private set; }
        public Field Captured { get; private set; }
        public Field Finish { get; private set; }

        public SimpleCapture(Field start, Field captured, Field finish)
        {
            Start = start;
            Captured = captured;
            Finish = finish;
        }
    }

    public class CaptureAction : ICheckersAction
    {
        public Field Start { get => Captures.First!.Value.Start; }

        public Field Finish { get => Captures.Last!.Value.Finish; }

        public LinkedList<SimpleCapture> Captures { get; private set; }

        public int CapturesCount { get; private set; }

        public CaptureAction(Field start, Field capture, Field finish)
        {
            CapturesCount = 1;
            Captures = new LinkedList<SimpleCapture>();
            Captures.AddFirst(new SimpleCapture(start, capture, finish));
        }

        public IEnumerable<Field> GetParticipatingFields()
        {
            yield return Start;
            foreach(SimpleCapture capture in Captures)
            {
                yield return capture.Finish;
            }
        }

        public IEnumerable<Field> GetPlayableFields()
        {
            foreach (SimpleCapture capture in Captures)
            {
                yield return capture.Finish;
            }
        }

        internal IEnumerable<Field> GetCapturedFields()
        {
            foreach (SimpleCapture capture in Captures)
            {
                yield return capture.Captured;
            }
        }

        public void CombineCapture(Field start, Field firstCapture)
        {
            SimpleCapture simpleCapture = new(start, firstCapture, Start);
            Captures.AddFirst(simpleCapture);
            CapturesCount++;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ICheckersAction);
        }

        public bool Equals(ICheckersAction? other)
        {
            if (other == null) return false;
            if (other is not CaptureAction captureAction) return false;
            if (!captureAction.Start.Equals(Start)) return false;
            if (!captureAction.Captures.SequenceEqual(Captures)) return false;
            return true;
        }

        public CheckersState PerformOn(CheckersState state)
        {
            return PerformOn(state, Piece.None);
        }

        public CheckersState PerformOn(CheckersState state, Piece substituteCapturedWith)
        {
            Piece capturer = state.GetPieceAt(Start);
            CheckersState newState = new(state);
            newState.SetPieceAt(Start, Piece.None);
            foreach (SimpleCapture capture in Captures)
            {
                newState.SetPieceAt(capture.Captured, substituteCapturedWith);
            }
            newState.SetPieceAtWithPossiblePromotion(Finish, capturer);
            newState.CurrentPlayer = state.CurrentPlayer == Player.One ? Player.Two : Player.One;
            newState.LastAction = this;

            return newState;
        }
    }
}

