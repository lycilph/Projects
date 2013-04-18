using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using LunchViewer.Infrastructure;

namespace LunchViewer
{
    public partial class App
    {
        private ApplicationController application_controller;

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);

            application_controller = container.GetExportedValue<ApplicationController>();
            application_controller.Initialize();
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            application_controller.Terminate();
        }
    }
}
