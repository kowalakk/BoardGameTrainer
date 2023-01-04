using Game.Checkers;
using Game.IGame;
using Gtk;

namespace BoardGameTrainer
{
    public static class Program
    {
        // Dodałem bezparametrowy interfejs IGame. Znalazłem, że tak się robi
        // Dołączyłem też referencję do warcabów jako przykład.
        // Trzeba ją potem usunąć - nasz program powinien przeszukiwać katalog
        // i sam dodawać gry z .dllek, a nie mieć bezpośrednią referencję
        private static IGame game = new Checkers();
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();
            
            var app = new Application("x.y.z", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);
            var win = new GameWindow(Gdk.WindowType.Toplevel);
            app.AddWindow(win);
            win.DefaultSize = new Gdk.Size(700, 500);
            var button = new Button("QUIT");
            win.Add(button);
            button.Show();
            win.Show();
            win.DeleteEvent += (sender, args) => Application.Quit();
            button.Clicked += (sender, args) => Application.Quit();
            Application.Run();
        }
    }
}
