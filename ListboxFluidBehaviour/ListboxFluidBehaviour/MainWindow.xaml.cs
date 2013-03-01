using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ListboxFluidBehaviour
{
    public partial class MainWindow
    {
        private readonly TimeSpan fade_out_duration;
        private readonly TimeSpan message_duration;

        public ObservableCollection<Item> Items
        {
            get { return (ObservableCollection<Item>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<Item>), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Items = new ObservableCollection<Item>();

            fade_out_duration = ((Duration)Resources["fade_out_duration"]).TimeSpan;
            message_duration = ((Duration)Resources["message_duration"]).TimeSpan;

            var desktop_working_area = SystemParameters.WorkArea;
            Height = desktop_working_area.Height - 20;
            Left = desktop_working_area.Right - Width - 10;
            Top = desktop_working_area.Bottom - Height - 10;
        }

        private void AddFrontClick(object sender, RoutedEventArgs e)
        {
            Items.Insert(0, new Item("Item - " + Guid.NewGuid(), Items.Remove, message_duration, fade_out_duration));
        }

        private void RemoveLastClick(object sender, RoutedEventArgs e)
        {
            if (Items.Count <= 0) return;

            Items.Last().Remove();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
