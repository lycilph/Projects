using System.Windows;

namespace CategorizationEngine
{
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            MainWindowViewModel vm = new MainWindowViewModel();
            MainWindow v = new MainWindow() {DataContext = vm};
            v.Show();
        }
    }
}
