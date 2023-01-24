namespace Game.Checkers
{
    public interface ICheckersInputState : IEquatable<ICheckersInputState> { }
    public class DefaultInputState : ICheckersInputState
    {
        public bool Equals(ICheckersInputState? other)
        {
            if (other == null) return false;
            return other is DefaultInputState;
        }
    }
    public class MarkedPieceInputState : ICheckersInputState
    {
        public Field MarkedField { get; private set; }
        public MarkedPieceInputState(Field markedField)
        {
            MarkedField = markedField;
        }

        public bool Equals(ICheckersInputState? other)
        {
            if (other == null) return false;
            if (other is not MarkedPieceInputState cIS) return false;
            return MarkedField.Equals(cIS.MarkedField);
        }
    }
    public class CaptureActionInProgressInputState : ICheckersInputState
    {
        public List<Field> VisitedFields { get; private set; }

        public CaptureActionInProgressInputState(IEnumerable<Field> visitedFields)
        {
            VisitedFields = new List<Field>(visitedFields);
        }

        public bool Equals(ICheckersInputState? other)
        {
            if (other == null) return false;
            if (other is not CaptureActionInProgressInputState cIS) return false;
            return VisitedFields.SequenceEqual(cIS.VisitedFields);
        }
    }
}