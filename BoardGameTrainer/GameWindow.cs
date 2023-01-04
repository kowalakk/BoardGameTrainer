using Gdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameTrainer
{
    internal class GameWindow : Gtk.Window
    {
        public GameWindow(nint raw) : base(raw)
        {
        }

        public GameWindow(WindowType type)
            : base(IntPtr.Zero)
        {
        }
        
        //public GameWindow(Window parent, WindowAttr attributes, int attributes_mask) : base(parent, attributes, attributes_mask)
        //{
        //}

        //public GameWindow(Window parent, WindowAttr attributes, WindowAttributesType attributes_mask) : base(parent, attributes, attributes_mask)
        //{
        //}


    }
}
