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
    public class CaptureActionInProgressCIS : ICheckersInputState
    {
        public IEnumerable<Field> VisitedFields { get; private set; }

        public CaptureActionInProgressCIS(IEnumerable<Field> visitedFields)
        {
            VisitedFields = visitedFields;
        }

        public bool Equals(ICheckersInputState? other)
        {
            if (other == null) return false;
            if (other is not CaptureActionInProgressCIS cIS) return false;
            return VisitedFields.SequenceEqual(cIS.VisitedFields);
        }
    }
}