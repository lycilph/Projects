using System.Collections.ObjectModel;
using System.Windows;
using Model;

namespace VisualStateManagerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Post> Posts
        {
            get { return (ObservableCollection<Post>)GetValue(PostsProperty); }
            set { SetValue(PostsProperty, value); }
        }
        public static readonly DependencyProperty PostsProperty =
            DependencyProperty.Register("Posts", typeof(ObservableCollection<Post>), typeof(MainWindow), new UIPropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Posts = new ObservableCollection<Post>(ModelFactory.GetRandomPosts(10000));
        }
    }
}
