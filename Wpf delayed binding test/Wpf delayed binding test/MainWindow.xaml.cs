using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_delayed_binding_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MainWindow), new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(OnTextChanged)));
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var main_window = d as MainWindow;
            if (main_window != null)
                main_window.UpdateCount++;
        }

        public int UpdateCount
        {
            get { return (int)GetValue(UpdateCountProperty); }
            set { SetValue(UpdateCountProperty, value); }
        }
        public static readonly DependencyProperty UpdateCountProperty =
            DependencyProperty.Register("UpdateCount", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
