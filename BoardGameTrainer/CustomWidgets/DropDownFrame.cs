using Gtk;

namespace BoardGameTrainer
{
    public class DropDownFrame<T> : Frame
    {
        public T Active => entries.ElementAt(DropDown.Active).Value;
        public event EventHandler Changed
        {
            add { DropDown.Changed += value; }
            remove { DropDown.Changed -= value; }
        }

        protected VBox Box { get; set; }
        protected ComboBoxText DropDown { get; }

        private readonly Dictionary<string, T> entries;

        public DropDownFrame(string label, Dictionary<string, T> entries) : base(label)
        {
            this.entries = entries;
            DropDown = new()
            {
                MarginStart = 3,
                MarginEnd = 3
            };
            foreach (string entry in entries.Keys)
            {
                DropDown.AppendText(entry);
            }
            DropDown.Active = 0;
            DropDown.Show();

            Box = new();
            Box.PackStart(DropDown, true, true, 3);
            Box.Show();

            Add(Box);

        }
        public void AddEntry(string entry)
        {
            DropDown.AppendText(entry);
            DropDown.Active = 0;
        }
    }
}
