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
            string title = "New Game";
            var app = new Application("x.y.z", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);
            var win = new Window(Gtk.WindowType.Toplevel);
            win.DefaultSize = new Gdk.Size(700, 500);
            app.AddWindow(win);
            var mainVBox = new Gtk.VBox();
            var panelHbox = new Gtk.HBox();
            var contentVBox = new Gtk.VBox();
            var newGameButton = new Button("New Game");
            var restartButton = new Button("Restart");
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);

            var mainTitle = new Gtk.Label(title);
            contentVBox.PackStart(mainTitle, false, false, 0);
            mainTitle.Show();

            mainVBox.PackStart(panelHbox, false, false, 0);
            panelHbox.Show();
            mainVBox.PackStart(contentVBox, false, false, 0);
            contentVBox.Show();
            win.Add(mainVBox);
            mainVBox.Show();
            newGameButton.Show();
            restartButton.Show();
            win.Show();
            win.DeleteEvent += (sender, args) => Application.Quit();
            //newGameButton.Clicked += (sender, args) => Application.Quit();
            Application.Run();
        }
    }
}
