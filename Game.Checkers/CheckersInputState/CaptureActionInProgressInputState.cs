namespace Game.Checkers
{
    public class CaptureActionInProgressInputState : ICheckersInputState
    {
        public List<int> VisitedFields { get; }

        public CaptureActionInProgressInputState(IEnumerable<int> visitedFields)
        {
            VisitedFields = new List<int>(visitedFields);
        }
    }
}