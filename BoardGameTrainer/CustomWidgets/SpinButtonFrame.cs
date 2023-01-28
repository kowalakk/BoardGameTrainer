using Gtk;

namespace BoardGameTrainer
{
    public class SpinButtonFrame : Frame
    {
        private readonly SpinButton spinButton;

        public double Value => spinButton.Value;

        public event EventHandler Changed
        {
            add { spinButton.Changed += value; }
            remove { spinButton.Changed -= value; }
        }
        public SpinButtonFrame(string label, 
            double min, 
            double max, 
            double step, 
            double initialValue, 
            string text) : base(label) 
        {
            spinButton = new(min, max, step)
            {
                Value = initialValue
            };
            spinButton.Show();

            Label textLabel = new(text);
            textLabel.Show();

            HBox box = new();
            box.Show();
            box.PackStart(spinButton, true, true, 5);
            box.PackStart(textLabel, false, false, 5);

            this.Add(box);
        }
    }
}
