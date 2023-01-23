namespace Game.Checkers
{
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
}