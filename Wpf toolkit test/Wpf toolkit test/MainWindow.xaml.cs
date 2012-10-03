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
using System.Windows.Controls.Primitives;
using Module_Interface;

namespace Wpf_toolkit_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AggregateComparer aggregate_comparer = null;

        //public ObservableCollection<Data> MasterData { get; private set; }
        public ObservableCollection<Data> MasterData
        {
            get { return (ObservableCollection<Data>)GetValue(MasterDataProperty); }
            set { SetValue(MasterDataProperty, value); }
        }
        public static readonly DependencyProperty MasterDataProperty =
            DependencyProperty.Register("MasterData", typeof(ObservableCollection<Data>), typeof(MainWindow), new UIPropertyMetadata(null));

        public ObservableCollection<Sorter> Sorters { get; private set; }

        public ICommand SortCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            #region DataGrid Data

            MasterData = new ObservableCollection<Data>();
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(1), "Item 1", 1.5));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(2), "Item 2", 2.2));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(3), "Item 3", 3.3));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(4), "Item 4", 4.4));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(5), "Item 5", 5.5));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(1), "Item 1", 1.4));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(2), "Item 2", 2.2));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(3), "Item 3", 3.3));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(4), "Item 4", 4.4));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(5), "Item 5", 5.5));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(1), "Item 1", 1.3));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(2), "Item 2", 2.2));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(3), "Item 3", 3.3));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(4), "Item 4", 4.4));
            MasterData.Add(new Data(DateTime.Now.Date.AddMonths(5), "Item 5", 5.5));

            for (int i = 0; i < 1000; i++)
                MasterData.Add(new Data(DateTime.Now.Date.AddMonths(6), "Item " + (100 + i), 42.0));

            #endregion

            var view = CollectionViewSource.GetDefaultView(MasterData) as ListCollectionView;
            aggregate_comparer = new AggregateComparer(view);

            Sorters = new ObservableCollection<Sorter>();

            SortCommand = new RelayCommand(Sort);

            DataContext = this;
        }

        private void Sort(object o)
        {
            Sorter s = o as Sorter;
            if (s == null)
                return;

            if (s.Direction == ListSortDirection.Ascending)
            {
                Sorters.Add(s);
            }
            else if (s.Direction == ListSortDirection.Descending)
            {
                var current_sorter = FindSorter(s.Name);
                current_sorter.Direction = ListSortDirection.Descending;
            }
            else
            {
                var current_sorter = FindSorter(s.Name);
                Sorters.Remove(current_sorter);
            }

            aggregate_comparer.AddComparer(new TextComparer());
            aggregate_comparer.Refresh();

            // Here the view should be refreshed
            // - Start a timer, 100 ms countdown
            // -- Tick should trigger a refresh
        }

        private Sorter FindSorter(string name)
        {
            foreach (var t in Sorters)
                if (t.Name == name)
                    return t;
            return null;
        }

        private void DetailsCheckBoxClick(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null)
                return;

            if (cb.IsChecked.HasValue && cb.IsChecked.Value)
                master_data_grid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            else
                master_data_grid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void FirstButtonClick(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(MasterData);
            view.MoveCurrentToFirst();
        }

        private void FilterToggleClick(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;
            if (tb == null)
                return;

            ICollectionView view = CollectionViewSource.GetDefaultView(MasterData);
            if (tb.IsChecked.HasValue && tb.IsChecked.Value)
                view.Filter = FilterOn1;
            else
                view.Filter = null;
        }

        private bool FilterOn1(object o)
        {
            // Return false if the input is NOT a model class or the text property is empty
            Data d = o as Data;
            if (d == null)
                return false;
            if (string.IsNullOrWhiteSpace(d.Text))
                return false;

            // Returns true if the input contains the filter text defined in Input1
            int index = d.Text.IndexOf("1", 0, StringComparison.InvariantCultureIgnoreCase);
            return index > -1;
        }
    }
}
