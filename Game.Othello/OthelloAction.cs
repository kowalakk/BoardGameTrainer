namespace Game.Othello
{
    public class OthelloAction : IEquatable<OthelloAction>
    {
        public OthelloAction((int, int) position, OthelloState.Field fieldContent)
        {
            if (position.Item1 >= 8 || position.Item2 >= 8
                || position.Item1 < 0 || position.Item2 < 0)
                throw new ArgumentException("Position can't fit on the board");
            Position = position;
            FieldContent = fieldContent;
        }

        public (int, int) Position { get; set; }
        public OthelloState.Field FieldContent { get; set; }

        public bool Equals(OthelloAction? other)
        {
            if (other == null) 
                return false; 
            if (this.Position != other.Position)
                return false;
            if (this.FieldContent!= other.FieldContent) 
                return false;
            return true;
        }
    }
}