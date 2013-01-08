using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Gui
{
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            MainWindowViewModel vm = new MainWindowViewModel();
            MainWindow w = new MainWindow() {DataContext = vm};
            w.Show();
        }
    }
}
