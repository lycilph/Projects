using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Wp_Checkbox_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ICommand ToggleItemCommand { get; private set; }

        public ObservableCollection<Item> Items { get; private set; }
        public ObservableCollection<Item> SelectedItems { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ToggleItemCommand = new RelayCommand(ToggleItem);

            Items = new ObservableCollection<Item>();
            Items.Add(new Item("Item 1"));
            Items.Add(new Item("Item 2"));
            Items.Add(new Item("Item 3"));
            Items.Add(new Item("Item 4"));
            Items.Add(new Item("Item 5"));

            SelectedItems = new ObservableCollection<Item>();
        }

        private void ToggleItem(object o)
        {
            Item item = o as Item;
            if (item != null)
            {
                if (item.Selected)
                    SelectedItems.Add(item);
                else
                    SelectedItems.Remove(item);
            }
        }
    }
}
