namespace Game.Checkers
{
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