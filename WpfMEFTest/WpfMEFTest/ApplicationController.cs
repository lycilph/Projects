using System.ComponentModel.Composition;

namespace WpfMEFTest
{
    [Export]
    public class ApplicationController
    {
        [Import]
        public IMainView MainView { get; set; }

        public void Start()
        {
            MainView.Show();
        }

        public void Stop()
        {
            MainView.Close();
        }
    }
}
