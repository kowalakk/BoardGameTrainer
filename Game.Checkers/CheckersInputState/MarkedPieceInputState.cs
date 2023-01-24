namespace Game.Checkers
{
    public class MarkedPieceInputState : ICheckersInputState
    {
        public ICheckersAction? PreviousAction { get;  }

        public Field MarkedField { get; }

        public MarkedPieceInputState(ICheckersAction? previousAction, Field markedField)
        {
            MarkedField = markedField;
            PreviousAction = previousAction;
        }
    }
}