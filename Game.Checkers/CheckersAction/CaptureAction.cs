using Game.IGame;

namespace Game.Checkers
{
    public struct SimpleCapture
    {
        public int Start { get; private set; }
        public int Captured { get; private set; }
        public int Finish { get; private set; }

        public SimpleCapture(int start, int captured, int finish)
        {
            Start = start;
            Captured = captured;
            Finish = finish;
        }
    }

    public class CaptureAction : ICheckersAction
    {
        public int Start { get => Captures.First!.Value.Start; }

        public int Finish { get => Captures.Last!.Value.Finish; }

        public LinkedList<SimpleCapture> Captures { get; private set; }

        public int CapturesCount { get; private set; }

        public CaptureAction(int start, int capture, int finish)
        {
            CapturesCount = 1;
            Captures = new LinkedList<SimpleCapture>();
            Captures.AddFirst(new SimpleCapture(start, capture, finish));
        }

        public IEnumerable<int> GetParticipatingFields()
        {
            yield return Start;
            foreach(SimpleCapture capture in Captures)
            {
                yield return capture.Finish;
            }
        }

        public IEnumerable<int> GetPlayableFields()
        {
            foreach (SimpleCapture capture in Captures)
            {
                yield return capture.Finish;
            }
        }

        internal IEnumerable<int> GetCapturedFields()
        {
            foreach (SimpleCapture capture in Captures)
            {
                yield return capture.Captured;
            }
        }

        public void CombineCapture(int start, int firstCapture)
        {
            SimpleCapture simpleCapture = new(start, firstCapture, Start);
            Captures.AddFirst(simpleCapture);
            CapturesCount++;
        }

        public CheckersState PerformOn(CheckersState state)
        {
            return PerformOn(state, Piece.None);
        }

        public CheckersState PerformOn(CheckersState state, Piece substituteCapturedWith)
        {
            Piece capturer = state.GetPieceAt(Start);
            CheckersState newState = new(state)
            {
                InsignificantActions = 0
            };
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

        public override bool Equals(object? other)
        {
            if (other == null) return false;
            if (other is not CaptureAction captureAction) return false;
            if (!captureAction.Start.Equals(Start)) return false;
            if (!captureAction.Captures.SequenceEqual(Captures)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ Finish.GetHashCode();
        }
    }
}

