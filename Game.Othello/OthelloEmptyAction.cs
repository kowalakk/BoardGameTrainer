using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Othello
{
    public class OthelloEmptyAction : OthelloAction
    {
        public override bool Equals(OthelloAction? other)
        {
            if (other is OthelloEmptyAction)
                return true;
            return false;
        }
    }
}
