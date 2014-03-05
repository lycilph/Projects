using Caliburn.Micro.ReactiveUI;
using System.ComponentModel.Composition;
using ReactiveUI;

namespace MetroWebbrowser.Shell.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : ReactiveScreen, IShell
    {
        private string home_url = "http://www.google.com";

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { this.RaiseAndSetIfChanged(ref _Address, value); }
        }

        public ShellViewModel()
        {
            DisplayName = "Shell";

            Home();
        }

        public void Home()
        {
            Address = home_url;
        }
    }
}
