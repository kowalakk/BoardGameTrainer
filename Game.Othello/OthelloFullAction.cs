namespace Game.Othello
{
    public class PotentialAction
    {
        public int[] piecesToFlip;

        public PotentialAction(PotentialAction potentialAction) : this(potentialAction.piecesToFlip) { }

        public PotentialAction(int[] piecesToFlip)
        {
            this.piecesToFlip = new int[8];
            Array.Copy(piecesToFlip, this.piecesToFlip, 8);
        }

        public bool IsEmpty()
        {
            for (int i = 0; i < 8; i++)
                if (piecesToFlip[i] != 0)
                    return false;
            return true;
        }
    }

    public class OthelloFullAction : PotentialAction, IOthelloAction
    {
        public (int, int) Position { get; set; }
        public OthelloState.Field FieldContent { get; set; }

        public OthelloFullAction((int, int) position,
            OthelloState.Field fieldContent,
            PotentialAction potentialAction) : base(potentialAction)
        {
            Position = position;
            FieldContent = fieldContent;
        }

        public bool Equals(IOthelloAction? other)
        {
            if (other == null) return false;
            if (other is OthelloEmptyAction)
                return false;
            OthelloFullAction action = (OthelloFullAction)other;
            if (Position != action.Position)
                return false;
            if (FieldContent != action.FieldContent)
                return false;
            return piecesToFlip.SequenceEqual(action.piecesToFlip);
        }

        public override int GetHashCode()
        {
            return Position.Item1 + 8 * Position.Item2;
        }
    }
}
