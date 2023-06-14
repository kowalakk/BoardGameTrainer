using Cairo;
using Game.IGame;
using LanguageExt;

namespace BoardGameTrainer
{
    internal class DefaultGameManager : IGameManager
    {
        private GameResult gameResult;

        public DefaultGameManager(GameResult gameResult)
        {
            this.gameResult = gameResult;
        }

        public void ComputeHints(CancellationToken token)
        {
        }

        public void DrawBoard(Context context, (int, int) numberOfHints)
        {

            if (gameResult == GameResult.InProgress)
                return;
            string text = string.Empty;
            if (gameResult == GameResult.PlayerOneWins)
            {
                text = "Player One Wins!";
                context.MoveTo(0, 0.5);
            }
            else if (gameResult == GameResult.PlayerTwoWins)
            {
                text = "Player Two Wins!";
                context.MoveTo(0, 0.5);
            }
            else if (gameResult == GameResult.Draw)
            {
                text = "A Draw!";
                context.MoveTo(0.25, 0.5);
            }
            context.SetSourceRGB(0, 0, 0);
            context.SelectFontFace("Sans", FontSlant.Italic, FontWeight.Bold);
            context.SetFontSize(0.12);
            context.ShowText(text);
            context.Stroke();
        }

        public (GameResult, bool) HandleMovement(double x, double y, bool isPlayer2Ai)
        {
            return (GameResult.InProgress, false);
        }

        public GameResult PerformOponentsMovement(GameResult gameResult)
        {
            return GameResult.InProgress;
        }

        public void Restart() { }
    }
}
