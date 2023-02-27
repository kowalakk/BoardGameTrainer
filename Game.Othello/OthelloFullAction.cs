using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Othello
{
    public struct PotentialAction
    {
        public int up { get; set; }
        public int down { get; set; }
        public int left { get; set; }
        public int right { get; set; }
        public int upLeft { get; set; }
        public int upRight { get; set; }
        public int downLeft { get; set; }
        public int downRight { get; set; }

        public PotentialAction(int up, int down, int left, int right, int upLeft,
            int upRight, int downLeft, int downRight)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.upLeft = upLeft;
            this.upRight = upRight;
            this.downLeft = downLeft;
            this.downRight = downRight;
        }

        public bool IsPotentialActionEmpty()
        {
            if (up == 0 && down == 0 && left == 0 && right == 0 && upLeft == 0 && upRight == 0
                && downLeft == 0 && downRight == 0)
                return true;
            return false;
        }
    }

    public class OthelloFullAction : OthelloAction
    {
        public OthelloFullAction((int, int) position, OthelloState.Field fieldContent, 
            PotentialAction potentialAction)
        {
            Position = position;
            FieldContent = fieldContent;
            this.up = potentialAction.up;
            this.down = potentialAction.down;
            this.left = potentialAction.left; 
            this.right = potentialAction.right;
            this.upLeft = potentialAction.upLeft;
            this.upRight = potentialAction.upRight;
            this.downLeft = potentialAction.downLeft;
            this.downRight = potentialAction.downRight;
        }

        public OthelloFullAction((int, int) position, OthelloState.Field fieldContent, int up, int down, int left, int right, 
            int upLeft, int upRight, int downLeft, int downRight)
        {
            Position = position;
            FieldContent = fieldContent;
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.upLeft = upLeft;
            this.upRight = upRight;
            this.downLeft = downLeft;
            this.downRight = downRight;
        }

        public (int, int) Position { get; set; }
        public OthelloState.Field FieldContent { get; set; }
        public int up { get; set; }
        public int down { get; set; }
        public int left { get; set; }
        public int right { get; set; }
        public int upLeft { get; set; }
        public int upRight { get; set; }
        public int downLeft { get; set; }
        public int downRight { get; set; }

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
            if (this.left != action.left || this.right != action.right || this.up != action.up 
                || this.down != action.down || this.upLeft != action.upLeft || this.upRight != action.upRight
                || this.downLeft != action.downLeft || this.downRight != action.downRight)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Position.Item1 + 8 * Position.Item2 + 8 * 8 * left + 8 * 8 * 8 * right + 8 * 8 * 8 * 8 * up + 8 * 8 * 8 * 8 * 8 * down;
        }
    }
}
