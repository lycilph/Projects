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

namespace Wpf_fluidmove_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Data> Data
        {
            get { return (ObservableCollection<Data>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ObservableCollection<Data>), typeof(MainWindow), new UIPropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Data = new ObservableCollection<Data>();
            for (int i = 0; i < 10; i++)
                Data.Add(new Data(string.Format("Item {0}", i),
                                  new SolidColorBrush(new Color()
                                                          {A = (byte)255, R = (byte) (i*5), G = (byte) (i*15), B = (byte) (i*10)})));
        }
    }
}
