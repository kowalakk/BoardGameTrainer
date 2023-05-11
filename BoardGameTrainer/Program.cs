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
