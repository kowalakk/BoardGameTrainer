using Game.IGame;
using Gtk;

namespace BoardGameTrainer
{
    internal class PlayersBox : VBox
    {
        private readonly CheckButtonsFrame numOfPlayersFrame;
        private readonly CheckButtonsFrame showHintsFrame;
        public PlayersBox(Dictionary<Player, bool> humanPlayers, Dictionary<Player, bool> showHints)
        {
            numOfPlayersFrame = new("Human players", humanPlayers);
            numOfPlayersFrame.Show();

            showHintsFrame = new("Show hints", showHints);
            showHintsFrame.Show();

            PackStart(numOfPlayersFrame, false, false, MainWindow.CustomMargin);
            PackStart(showHintsFrame, false, false, MainWindow.CustomMargin);

            numOfPlayersFrame.FirstCheckbox.Clicked += (sender, args) =>
            {
                showHintsFrame.FirstCheckbox.Sensitive = numOfPlayersFrame.FirstCheckbox.Active;
            };
            numOfPlayersFrame.SecondCheckbox.Clicked += (sender, args) =>
            {
                showHintsFrame.SecondCheckbox.Sensitive = numOfPlayersFrame.SecondCheckbox.Active;
            };
        }
    }
}
