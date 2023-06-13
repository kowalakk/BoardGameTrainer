using Gtk;

namespace BoardGameTrainer
{
    public class DropDownFrame : Frame
    {
        private readonly ComboBox dropDown;

        public event EventHandler Changed
        {
            add { dropDown.Changed += value; }
            remove { dropDown.Changed -= value; }
        }

        public int Active => dropDown.Active;

        public DropDownFrame(string label, string [] entries) : base(label) 
        {
            dropDown = new(entries)
            {
                Active = 0
            };
            dropDown.Show();

            HBox box = new();
            box.Show();
            box.PackStart(dropDown, true, true, 5);

            Add(box);
        }
    }
}
