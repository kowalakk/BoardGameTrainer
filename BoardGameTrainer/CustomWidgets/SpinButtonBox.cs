using Gtk;

namespace BoardGameTrainer
{
    public class SpinButtonBox : HBox
    {
        private readonly SpinButton spinButton;

        public double Value => spinButton.Value;
        public bool IsEditable => spinButton.IsEditable;

        public event EventHandler Changed
        {
            add { spinButton.Changed += value; }
            remove { spinButton.Changed -= value; }
        }

        public SpinButtonBox
        ( 
            double min, 
            double max, 
            double step, 
            double initialValue, 
            string text
        ) : base() 
        {
            spinButton = new(min, max, step)
            {
                Value = initialValue
            };
            spinButton.Show();

            Label textLabel = new(text);
            textLabel.Show();

            PackStart(spinButton, true, true, 3);
            PackStart(textLabel, false, false, 3);
        }
    }
}
