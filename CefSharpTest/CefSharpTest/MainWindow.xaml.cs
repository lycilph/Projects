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
        }
    }
}
