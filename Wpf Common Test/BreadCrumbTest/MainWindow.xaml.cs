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

namespace BreadCrumbTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Items
        {
            get { return (ObservableCollection<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<string>), typeof(MainWindow), new UIPropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Items = new ObservableCollection<string> {"Item 1", "Item 2", "Item 3"};
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            Items.Add("Item " + (Items.Count+1));
        }
    }
}
