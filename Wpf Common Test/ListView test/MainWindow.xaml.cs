using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Model;
using System.Windows.Data;
using System.ComponentModel;
using ListView_test.Comparers;
using System.Linq;
using SortUtils;

namespace ListView_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AggregateComparer<Post> aggregate_comparer;

        public ObservableCollection<Post> Posts
        {
            get { return (ObservableCollection<Post>)GetValue(PostsProperty); }
            set { SetValue(PostsProperty, value); }
        }
        public static readonly DependencyProperty PostsProperty =
            DependencyProperty.Register("Posts", typeof(ObservableCollection<Post>), typeof(MainWindow), new UIPropertyMetadata(null));

        public ICommand SortCommand { get; private set; }
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Posts = new ObservableCollection<Post>(ModelFactory.GetRandomPosts(10000));
            SortCommand = new RelayCommand(Sort);

            aggregate_comparer = new AggregateComparer<Post>(CollectionViewSource.GetDefaultView(Posts) as ListCollectionView, new ComparerFactory());
        }

        private void Sort(object o)
        {
            SortArgument sort_argument = o as SortArgument;
            if (sort_argument != null)
            {
                aggregate_comparer.Update(sort_argument);
            }
        }
    }
}
