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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Wpf_test_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Tuple<string, int>> Data
        {
            get { return (ObservableCollection<Tuple<string, int>>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ObservableCollection<Tuple<string, int>>), typeof(MainWindow), new UIPropertyMetadata(new ObservableCollection<Tuple<string, int>>()));

        public MainWindow()
        {
            InitializeComponent();

            Data.Add(Tuple.Create("Runtime string 1", 1));
            Data.Add(Tuple.Create("Runtime string 2", 2));
            Data.Add(Tuple.Create("Runtime string 3", 3));

            DataContext = this;                
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DesignAndRunTimeDatacontext data_source = FindResource("data_source") as DesignAndRunTimeDatacontext;
            if (data_source != null)
            {
                data_source.RuntimeDatacontext = Data;
                data_source.Refresh();
            }
        }
    }
}
