using Game.IGame;
using Gtk;

namespace BoardGameTrainer
{
    internal class PlayersBox : VBox
    {
        private readonly CheckButtonsFrame numOfPlayersFrame = new("Human players");
        private readonly CheckButtonsFrame showHintsFrame = new("Show hints");
        public PlayersBox(Dictionary<Player, bool> humanPlayers, Dictionary<Player, bool> showHints)
        {
            numOfPlayersFrame.FirstClicked += (sender, args) => { humanPlayers[Player.One] = numOfPlayersFrame.FirstActive; };
            numOfPlayersFrame.SecondClicked += (sender, args) => { humanPlayers[Player.Two] = numOfPlayersFrame.SecondActive; };
            numOfPlayersFrame.Show();

            showHintsFrame.FirstClicked += (sender, args) => { showHints[Player.One] = showHintsFrame.FirstActive; };
            showHintsFrame.SecondClicked += (sender, args) => { showHints[Player.Two] = showHintsFrame.SecondActive; };
            showHintsFrame.Show();

            PackStart(numOfPlayersFrame, false, false, 3);
            PackStart(showHintsFrame, false, false, 3);
        }
    }
}
