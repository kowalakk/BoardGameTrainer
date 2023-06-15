using Game.IGame;
using Gtk;

namespace BoardGameTrainer
{
    public class GameTrainerApplication : Application
    {
        public GameTrainerApplication() : base("x.y.z", GLib.ApplicationFlags.None)
        {
            Register(GLib.Cancellable.Current);

            MainWindow mainWindow = new(this);
            mainWindow.DeleteEvent += (sender, args) => Quit();
            mainWindow.Show();

            AddWindow(mainWindow);
        }
    }
}