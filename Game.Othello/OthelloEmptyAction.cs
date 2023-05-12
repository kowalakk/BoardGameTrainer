using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Othello
{
    public class OthelloEmptyAction : IOthelloAction
    {
        public bool Equals(IOthelloAction? other)
        {
            if (other is OthelloEmptyAction)
                return true;
            return false;
        }
    }
}
