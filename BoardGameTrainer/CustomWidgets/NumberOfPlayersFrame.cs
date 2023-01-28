using Gtk;

namespace BoardGameTrainer
{
    internal class NumberOfPlayersFrame : Frame
    {
        private RadioButton firstButton;

        private RadioButton secondButton;

        public event EventHandler FirstClicked
        {
            add { firstButton.Clicked += value; }
            remove { firstButton.Clicked -= value; }
        }

        public event EventHandler SecondClicked
        {
            add { secondButton.Clicked += value; }
            remove { secondButton.Clicked -= value; }
        }

        public NumberOfPlayersFrame() : base("Number of players")
        {
            firstButton = new(null, "One Player");
            secondButton = new(firstButton, "Two Players");

            firstButton.Show();
            secondButton.Show();

            HBox box = new();
            box.Show();
            box.PackStart(firstButton, false, false, 5);
            box.PackStart(secondButton, false, false, 5);
            this.Add(box);
        }
    }
}
