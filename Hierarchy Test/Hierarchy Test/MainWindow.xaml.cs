using System.Windows;
using System.Windows.Input;
using NLog;

namespace Hierarchy_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        public ICommand CloseCommand { get; private set; }
        public ICommand ToggleLogCommand { get; private set; }

        public MainWindow()
        {
            log.Debug("Constructor");

            InitializeComponent();
            CloseCommand = new RelayCommand(_ => this.Close());
            ToggleLogCommand = new RelayCommand(_ => this.ToggleLog());
            DataContext = this;
        }

        private void ToggleLog()
        {
            if (LogManager.IsLoggingEnabled())
            {
                log.Debug("Disabling log");
                LogManager.DisableLogging();
            }
            else
            {
                LogManager.EnableLogging();
                log.Debug("Enabling log");
            }
        }

        private void SelectedNodeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Node n = e.NewValue as Node;

            log.Debug("Selected node changed to: " + n.Name);
        }

        protected override void OnClosed(System.EventArgs e)
        {
            log.Debug("Closed");
            base.OnClosed(e);
        }
    }
}
