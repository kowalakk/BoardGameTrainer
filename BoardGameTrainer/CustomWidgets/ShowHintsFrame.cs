using Gtk;

namespace BoardGameTrainer
{
    internal class ShowHintsFrame : Frame
    {
        private readonly CheckButton firstCheckbox;

        private readonly CheckButton secondCheckbox;

        public bool FirstActive => firstCheckbox.Active;
        public bool SecondActive => secondCheckbox.Active;

        public event EventHandler FirstClicked
        {
            add { firstCheckbox.Clicked += value; }
            remove { firstCheckbox.Clicked -= value; }
        }

        public event EventHandler SecondClicked
        {
            add { secondCheckbox.Clicked += value; }
            remove { secondCheckbox.Clicked -= value; }
        }
        public ShowHintsFrame() : base("Show hints") 
        {
            firstCheckbox = new CheckButton("Player 1");
            firstCheckbox.Show();
            firstCheckbox.Active = true;

            secondCheckbox = new CheckButton("Player 2");
            secondCheckbox.Show();
            secondCheckbox.Active = true;

            HBox box = new();
            box.Show();
            box.PackStart(firstCheckbox, false, false, 5);
            box.PackStart(secondCheckbox, false, false, 5);

            this.Add(box);
        }
    }
}
