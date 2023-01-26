namespace Game.Checkers
{
    public class CaptureActionInProgressInputState : ICheckersInputState
    {
        public ICheckersAction? PreviousAction { get; }

        public List<Field> VisitedFields { get; }

        public CaptureActionInProgressInputState(ICheckersAction? previousAction, IEnumerable<Field> visitedFields)
        {
            VisitedFields = new List<Field>(visitedFields);
            PreviousAction = previousAction;
        }
    }
}