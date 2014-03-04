using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CefSharp1test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string data;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CefSharp1test.theme.css"))
            using (var reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }
            data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));

            var bs = new BrowserSettings
            {
                UserStyleSheetEnabled = true,
                UserStyleSheetLocation = @"data:text/css;charset=utf-8;base64," + data
            };
            var wv = new WebView("http://www.google.com", bs);
            layout_root.Children.Add(wv);
        }
    }
}
