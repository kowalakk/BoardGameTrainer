namespace Game.Checkers
{
    public interface ICheckersInputState : IEquatable<ICheckersInputState> { }
    public class IdleCIS : ICheckersInputState
    {
        public bool Equals(ICheckersInputState? other)
        {
            if (other == null) return false;
            return other is IdleCIS;
        }
    }
    public class MarkedPieceCIS : ICheckersInputState
    {
        public Field MarkedField { get; private set; }
        public MarkedPieceCIS(Field markedField)
        {
            MarkedField = markedField;
        }

        public bool Equals(ICheckersInputState? other)
        {
            if (other == null) return false;
            if (other is not MarkedPieceCIS cIS) return false;
            return MarkedField.Equals(cIS.MarkedField);
        }
    }
    public class CaptureActionsInProgressCIS : ICheckersInputState
    {
        public IEnumerable<(CaptureAction, double)> RatedCaptureActions { get; private set; }
        public Field MovingPiecePosition { get; private set; }
        public CaptureActionsInProgressCIS(Field movingPiecePosition, IEnumerable<(CaptureAction, double)> captureActions)
        {
            MovingPiecePosition = movingPiecePosition;
            RatedCaptureActions = captureActions;
        }
        public IEnumerable<(CaptureAction, double)> FilterCaptureActionsByField(Field field)
        {
            return RatedCaptureActions.Where(tuple => tuple.Item1.GetVisitedFields().Contains(field));
        }
        public bool Equals(ICheckersInputState? other)
        {
            if (other == null) return false;
            if (other is not CaptureActionsInProgressCIS cIS) return false;
            if (!MovingPiecePosition.Equals(cIS.MovingPiecePosition)) return false;
            if (!RatedCaptureActions.SequenceEqual(cIS.RatedCaptureActions)) return false;
            return true;
        }
    }
}