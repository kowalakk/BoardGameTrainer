using Game.Checkers;
using Game.IGame;
using Gdk;
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
            var mainWindow = createMainWindow();
            app.AddWindow(mainWindow);
            
            Application.Run();
        }

        public static Gtk.Window createMainWindow()
        {
            string title = "New Game";
            var win = new Gtk.Window(Gtk.WindowType.Toplevel);
            win.DefaultSize = new Gdk.Size(700, 500);
            
            var mainVBox = new Gtk.VBox();
            var panelHbox = new Gtk.HBox();
            var titleAndContentVBox = new Gtk.VBox();

            var contentHBox = new Gtk.HBox();
            var boardPixbuf = new Gdk.Pixbuf("..\\..\\..\\Tulips.jpg", 200, 200);

            // zamiast tego będzie przypisanie do boardImage wartości game.drawBoard()
            var boardImage = new Gtk.Image(boardPixbuf);
            contentHBox.PackStart(boardImage, false, false, 0);
            boardImage.Show();

            var newGameButton = new Button("New Game");
            var restartButton = new Button("Restart");
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);

            var mainTitle = new Gtk.Label(title);
            titleAndContentVBox.PackStart(mainTitle, false, false, 0);
            titleAndContentVBox.PackStart(contentHBox, false, false, 0);
            mainTitle.Show();
            contentHBox.Show();

            mainVBox.PackStart(panelHbox, false, false, 0);
            panelHbox.Show();
            mainVBox.PackStart(titleAndContentVBox, false, false, 0);
            titleAndContentVBox.Show();
            win.Add(mainVBox);
            mainVBox.Show();
            newGameButton.Show();
            restartButton.Show();
            win.Show();
            win.DeleteEvent += (sender, args) => Application.Quit();
            return win;
        }
    }
}
