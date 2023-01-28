namespace Game.Checkers
{
    public class MarkedPieceInputState : ICheckersInputState
    {
        public int MarkedField { get; }

        public MarkedPieceInputState(int markedField)
        {
            MarkedField = markedField;
        }
    }
}