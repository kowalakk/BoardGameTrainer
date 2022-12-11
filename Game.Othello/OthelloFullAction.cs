using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Othello
{
    public class OthelloFullAction : OthelloAction
    {
        public OthelloFullAction((int, int) position, OthelloState.Field fieldContent)
        {
            if (position.Item1 >= 8 || position.Item2 >= 8
                || position.Item1 < 0 || position.Item2 < 0)
                throw new ArgumentException("Position can't fit on the board");
            Position = position;
            FieldContent = fieldContent;
        }

        public (int, int) Position { get; set; }
        public OthelloState.Field FieldContent { get; set; }

        public override bool Equals(OthelloAction other)
        {
            if (other.GetType() == typeof(OthelloEmptyAction))
                return false;
            if (this.Position != ((OthelloFullAction)other).Position)
                return false;
            if (this.FieldContent != ((OthelloFullAction)other).FieldContent)
                return false;
            return true;
        }
    }
}
