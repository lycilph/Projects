using Caliburn.Micro.ReactiveUI;
using System.ComponentModel.Composition;

namespace MetroWebbrowser.Shell.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : ReactiveScreen, IShell
    {
        public ShellViewModel()
        {
            DisplayName = "Shell";
        }
    }
}
