namespace Game.Othello
{
    public class OthelloAction : IEquatable<OthelloAction>
    {
        public (int, int) Position { get; set; }
        public OthelloState.Field FieldContent { get; set; }

        public bool Equals(OthelloAction? other)
        {
            if (this == null || other == null) 
                return false; 
            if (this.Position != other.Position)
                return false;
            if (this.FieldContent!= other.FieldContent) 
                return false;
            return true;
        }
    }
}