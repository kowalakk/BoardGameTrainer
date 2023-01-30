using Ai;
using Game.Checkers;
using Game.IGame;
using Game.Othello;
using Gdk;
using Gtk;

namespace BoardGameTrainer
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            _ = new GameTrainerApplication();

            Application.Run();
        }
    }
}
