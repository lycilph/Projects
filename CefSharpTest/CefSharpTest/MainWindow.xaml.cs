using CefSharp;
using CefSharp.Wpf;
using System.ComponentModel;
using System.Windows;

namespace CefSharpTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string home_url = "http://www.google.com";

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public IWpfWebBrowser WebBrowser
        {
            get { return (IWpfWebBrowser)GetValue(WebBrowserProperty); }
            set { SetValue(WebBrowserProperty, value); }
        }
        public static readonly DependencyProperty WebBrowserProperty =
            DependencyProperty.Register("WebBrowser", typeof(IWpfWebBrowser), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Address = home_url;
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            WebBrowser.Address = home_url;
        }
    }
}
