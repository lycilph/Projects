using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using DragAndDrop;
using NLog;

namespace Wpf_test
{
    /// <summary>
    /// Interaction logic for DataTemplatesWindow.xaml
    /// </summary>
    public partial class DataTemplatesWindow : Window, IDropTarget
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private DefaultDropHandler default_drop_handler = new DefaultDropHandler();

        public ObservableCollection<Post> Posts
        {
            get { return (ObservableCollection<Post>)GetValue(PostsProperty); }
            set { SetValue(PostsProperty, value); }
        }
        public static readonly DependencyProperty PostsProperty =
            DependencyProperty.Register("Posts", typeof(ObservableCollection<Post>), typeof(DataTemplatesWindow), new UIPropertyMetadata(new ObservableCollection<Post>()));

        public ObservableCollection<Category> Categories
        {
            get { return (ObservableCollection<Category>)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(ObservableCollection<Category>), typeof(DataTemplatesWindow), new UIPropertyMetadata(new ObservableCollection<Category>()));

        public DataTemplatesWindow()
        {
            log.Debug("DataTemplatesWindow constructor");

            Categories.Add(new Category("Category 1"));
            Categories.Add(new Category("Category 2"));
            Categories.Add(new Category("Category 3"));
            Posts.Add(new Post("Post 1"));
            Posts.Add(new Post("Post 2", Categories[0], CategoryMatchType.Auto));
            Posts.Add(new Post("Post 3", Categories[1], CategoryMatchType.Manual));
            Posts.Add(new Post("Post 4", Categories[2], CategoryMatchType.Manual));
            Posts.Add(new Post("Post 5"));

            InitializeComponent();
            DataContext = this;
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        void IDropTarget.DragOver(DropInfo drop_info)
        {
            log.Trace("DragOver");

            default_drop_handler.DragOver(drop_info);
        }

        void IDropTarget.Drop(DropInfo drop_info)
        {
            log.Trace("Drop");

            default_drop_handler.Drop(drop_info);

            if (drop_info.Data is Post)
            {
                var posts_view = CollectionViewSource.GetDefaultView(Posts);
                posts_view.MoveCurrentTo(drop_info.Data);
            }
        }
    }
}
