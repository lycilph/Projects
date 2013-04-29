using System.Windows;

namespace WpfPathTrimmingTextBox
{
    public partial class MainWindow
    {
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Path = @"C:\Users\kbn_mml\AppData\Roaming\LunchViewer\menu_repository.json";
        }
    }
}
