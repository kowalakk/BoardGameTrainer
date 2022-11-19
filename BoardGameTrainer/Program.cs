using Gtk;

namespace BoardGameTrainer
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("x.y.z", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);
            var win = new Window(Gtk.WindowType.Toplevel);
            app.AddWindow(win);
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
