using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Othello
{
    public class OthelloFullAction : OthelloAction
    {
        public OthelloFullAction((int, int) position, OthelloState.Field fieldContent, int up, int down, int left, int right)
        {
            Position = position;
            FieldContent = fieldContent;
            this.up = up; this.down = down; this.left = left; this.right = right;
        }

        public (int, int) Position { get; set; }
        public OthelloState.Field FieldContent { get; set; }
        public int up { get; set; }
        public int down { get; set; }
        public int left { get; set; }
        public int right { get; set; }

        public override bool Equals(OthelloAction? other)
        {
            if(other == null) return false;
            if (other is OthelloEmptyAction)
                return false;
            OthelloFullAction action = (OthelloFullAction) other;
            if (this.Position != action.Position)
                return false;
            if (this.FieldContent != action.FieldContent)
                return false;
            if (this.left != action.left || this.right != action.right || this.up != action.up || this.down != action.down)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Position.Item1 + 8 * Position.Item2 + 8 * 8 * left + 8 * 8 * 8 * right + 8 * 8 * 8 * 8 * up + 8 * 8 * 8 * 8 * 8 * down;
        }
    }
}
