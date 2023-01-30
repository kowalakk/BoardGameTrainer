namespace Game.Checkers
{
    public class MarkedPieceInputState : ICheckersInputState
    {
        public Field MarkedField { get; }

        public MarkedPieceInputState(Field markedField)
        {
            MarkedField = markedField;
        }
    }
}