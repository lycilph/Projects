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
using System.ComponentModel;

namespace Test
{
    public partial class MainWindow : Window
    {
        private Model m;
        private ViewModel vm;

        public MainWindow()
        {
            InitializeComponent();

            m = new Model(1, 2);
            vm = new ViewModel(m);

            DataContext = vm;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            vm.Unsubscribe();
            base.OnClosing(e);
        }

        private void DoButtonClick(object sender, RoutedEventArgs e)
        {
            m.Prop1++;
            m.Prop2++;
        }

        private void NewModelClick(object sender, RoutedEventArgs e)
        {
            vm.WrappedModel = new ModelWrapped(new Model(20, 30));
        }
    }
}
