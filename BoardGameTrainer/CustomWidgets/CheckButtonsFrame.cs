using Game.IGame;
using Gtk;

namespace BoardGameTrainer
{
    internal class CheckButtonsFrame : Frame
    {
        public CheckButton FirstCheckbox { get; }
        public CheckButton SecondCheckbox { get; }

        public CheckButtonsFrame(string label, Dictionary<Player, bool> dict) : base(label)
        {
            FirstCheckbox = new CheckButton("Player 1");
            FirstCheckbox.Show();
            FirstCheckbox.Active = true;

            SecondCheckbox = new CheckButton("Player 2");
            SecondCheckbox.Show();
            SecondCheckbox.Active = true;

            HBox box = new();
            box.Show();
            box.PackStart(FirstCheckbox, false, false, 5);
            box.PackStart(SecondCheckbox, false, false, 5);

            Add(box);

            FirstCheckbox.Clicked += (sender, args) => { dict[Player.One] = FirstCheckbox.Active; };
            SecondCheckbox.Clicked += (sender, args) => { dict[Player.Two] = SecondCheckbox.Active; };
        }
    }
}
