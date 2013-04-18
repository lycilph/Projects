using System.ComponentModel.Composition;

namespace WpfMEFTest
{
    [Export(typeof (IMainView))]
    public partial class MainWindow : IMainView
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
