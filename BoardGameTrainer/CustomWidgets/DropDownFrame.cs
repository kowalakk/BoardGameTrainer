using Gtk;

namespace BoardGameTrainer
{
    public class DropDownFrame<T> : Frame
    {
        private Dictionary<string, T> Entries { get; }

        private ComboBox DropDown { get; }

        public event EventHandler Changed
        {
            add { DropDown.Changed += value; }
            remove { DropDown.Changed -= value; }
        }

        public T Active => Entries.ElementAt(DropDown.Active).Value;

        public DropDownFrame(string label, Dictionary<string, T> entries) : base(label)
        {
            Entries = entries;
            DropDown = new(Entries.Keys.ToArray())
            {
                Active = 0
            };
            DropDown.Show();

            HBox box = new();
            box.Show();
            box.PackStart(DropDown, true, true, 5);

            Add(box);
        }
    }
}
