namespace Game.Checkers
{
    public class CaptureActionInProgressInputState : ICheckersInputState
    {
        public List<Field> VisitedFields { get; }

        public CaptureActionInProgressInputState(IEnumerable<Field> visitedFields)
        {
            VisitedFields = new List<Field>(visitedFields);
        }
    }
}