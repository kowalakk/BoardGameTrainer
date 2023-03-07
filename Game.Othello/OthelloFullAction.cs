namespace Game.Othello
{
    public readonly struct PotentialAction
    {
        public int[] PiecesToFlip { get; }

        public PotentialAction(int[] piecesToFlip)
        {
            PiecesToFlip = new int[8];
            Array.Copy(piecesToFlip, PiecesToFlip, 8);
        }

        public bool IsEmpty()
        {
            for (int i = 0; i < 8; i++)
                if (PiecesToFlip[i] != 0)
                    return false;
            return true;
        }
    }

    public class OthelloFullAction : IOthelloAction
    {
        public (int, int) Position { get; set; }
        public OthelloState.Field FieldContent { get; set; }
        public int[] PiecesToFlip { get; }

        public OthelloFullAction((int, int) position,
            OthelloState.Field fieldContent,
            PotentialAction potentialAction)
        {
            Position = position;
            FieldContent = fieldContent;
            PiecesToFlip = potentialAction.PiecesToFlip;
        }
        public OthelloFullAction((int, int) position,
            OthelloState.Field fieldContent,
            params int[] piecesToFlip)
        {
            Position = position;
            FieldContent = fieldContent;
            PiecesToFlip = piecesToFlip;
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
            return PiecesToFlip.SequenceEqual(action.PiecesToFlip);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode() + FieldContent.GetHashCode() + PiecesToFlip.GetHashCode();
        }
    }
}
