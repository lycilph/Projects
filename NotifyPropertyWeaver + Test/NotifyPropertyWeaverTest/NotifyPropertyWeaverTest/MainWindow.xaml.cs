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

namespace NotifyPropertyWeaverTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Data data = new Data();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = data;
        }

        private void Change1Click(object sender, RoutedEventArgs e)
        {
            data.Count += 1;
        }

        private void Change2Click(object sender, RoutedEventArgs e)
        {
            data.Text += "1";
        }
    }
}
