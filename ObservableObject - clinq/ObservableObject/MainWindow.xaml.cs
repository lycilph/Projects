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

namespace ObservableObject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel view_model;

        public MainWindow()
        {
            InitializeComponent();

            view_model = new ViewModel(new Model(1, 2));

            DataContext = view_model;
        }

        private void AddProp1ButtonClick(object sender, RoutedEventArgs e)
        {
            view_model.WrappedModel.Prop1++;
        }

        private void ChangeModelButtonClick(object sender, RoutedEventArgs e)
        {
            view_model.WrappedModel = new Model(5, 6);
        }
    }
}
