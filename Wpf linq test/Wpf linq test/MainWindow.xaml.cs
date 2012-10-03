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
using Module_Interface;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Threading;
using System.Globalization;

namespace Wpf_linq_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Profile profile = new Profile();
        private DispatcherTimer timer = new DispatcherTimer();

        public ICommand CloseCommand { get; set; }

        public ObservableCollection<Node> MasterNodesBarChart
        {
            get { return (ObservableCollection<Node>)GetValue(MasterNodesProperty); }
            set { SetValue(MasterNodesProperty, value); }
        }
        public static readonly DependencyProperty MasterNodesProperty =
            DependencyProperty.Register("MasterNodesBarChart", typeof(ObservableCollection<Node>), typeof(MainWindow), new UIPropertyMetadata(null));

        public ObservableCollection<Node> MasterNodesLineChart
        {
            get { return (ObservableCollection<Node>)GetValue(MasterNodesLineChartProperty); }
            set { SetValue(MasterNodesLineChartProperty, value); }
        }
        public static readonly DependencyProperty MasterNodesLineChartProperty =
            DependencyProperty.Register("MasterNodesLineChart", typeof(ObservableCollection<Node>), typeof(MainWindow), new UIPropertyMetadata(null));

        public Node CurrentNode
        {
            get { return (Node)GetValue(CurrentNodeProperty); }
            set { SetValue(CurrentNodeProperty, value); }
        }
        public static readonly DependencyProperty CurrentNodeProperty =
            DependencyProperty.Register("CurrentNode", typeof(Node), typeof(MainWindow), new UIPropertyMetadata(null, new PropertyChangedCallback(OnCurrentNodeChanged)));
        private static void OnCurrentNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow main_window = d as MainWindow;
            if (main_window == null)
                return;

            Node n = e.NewValue as Node;
            if (n == null)
                return;

            main_window.StatusText = n.Name;
        }

        public string StatusText
        {
            get { return (string)GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }
        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register("StatusText", typeof(string), typeof(MainWindow), new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(OnStatusTextChanged)));
        private static void OnStatusTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow main_window = d as MainWindow;
            if (main_window == null)
                return;

            if (!string.IsNullOrEmpty(e.NewValue as string))
                if (main_window.timer.IsEnabled)
                {
                    main_window.timer.Stop();
                    main_window.timer.Start();
                }
                else
                    main_window.timer.Start();
        }

        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0,0,2);
             
            CloseCommand = new RelayCommand(_ => Close());

            CreateData();
            
            DataContext = this;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timer.Stop();
 	        StatusText = string.Empty;
        }

        private void CategorizeBarChart()
        {
            // Make master list
            MasterNodesBarChart = new ObservableCollection<Node>();
            MasterNodesBarChart.Add(new Node("All", profile.GetAllPosts()));
            foreach (Account a in profile.Accounts)
                MasterNodesBarChart.Add(new Node(a.Name, a.Posts));

            // Add year subnodes
            foreach (Node master_node in MasterNodesBarChart)
            {
                var year_nodes = from p in master_node.Posts
                                 group p by p.Date.Year into g
                                 select new Node(g.Key.ToString(), g);
                foreach (Node year_node in year_nodes)
                    master_node.Subnodes.Add(year_node);

                // Add month subnodes
                foreach (Node year_node in master_node.Subnodes)
                {
                    DateTimeFormatInfo fi = new DateTimeFormatInfo();

                    var month_nodes = from p in year_node.Posts
                                      group p by p.Date.Month into g
                                      select new Node(fi.GetMonthName(g.Key), g);
                    foreach (Node month_node in month_nodes)
                        year_node.Subnodes.Add(month_node);
                }
            }

            // Categorize nodes recursively
            foreach (var node in MasterNodesBarChart)
                Node.GroupByCategoryRecursively(node);
        }

        private void CategorizeLineChart()
        {
            Stopwatch stop_watch = new Stopwatch();
            stop_watch.Start();

            // Make master list
            MasterNodesLineChart = new ObservableCollection<Node>();
            MasterNodesLineChart.Add(new Node("All", profile.GetAllPosts()));
            foreach (Account a in profile.Accounts)
                MasterNodesLineChart.Add(new Node(a.Name, a.Posts));

            // Add category subnodes
            foreach (Node master_node in MasterNodesLineChart)
            {
                var category_nodes = from p in master_node.Posts
                                     orderby p.Match.Name
                                     group p by p.Match into g
                                     select new Node(g.Key.Name, g);
                foreach (Node category_node in category_nodes)
                    master_node.Subnodes.Add(category_node);

                foreach (Node category_node in master_node.Subnodes)
                {
                    DateTimeFormatInfo fi = new DateTimeFormatInfo();

                    var data = from p in category_node.Posts
                               orderby p.Date
                               group p by string.Format("{0}, {1}", p.Date.Year, fi.GetMonthName(p.Date.Month)) into g
                               select new PlotData(g.Key, g.Sum(p => p.Value));
                    category_node.Data = new ObservableCollection<PlotData>(data);
                }
            }

            stop_watch.Stop();
            StatusText = "Categorization took: " + stop_watch.ElapsedMilliseconds + " milliseconds";
        }

        private void CreateData()
        {
            List<Category> categories = new List<Category>();
            categories.Add(new Category("Category 1"));
            categories.Add(new Category("Category 2"));
            categories.Add(new Category("Category 3"));

            List<Post> posts = new List<Post>();
            posts.Add(new Post(DateTime.Now.AddYears(0).AddMonths(0), "Post 1", 1.1, categories[0]));
            posts.Add(new Post(DateTime.Now.AddYears(0).AddMonths(1), "Post 2", 2.2, categories[1]));
            posts.Add(new Post(DateTime.Now.AddYears(0).AddMonths(2), "Post 3", 3.3, categories[2]));

            posts.Add(new Post(DateTime.Now.AddYears(0).AddMonths(1), "Post 4", 4.4, categories[1]));
            posts.Add(new Post(DateTime.Now.AddYears(1).AddMonths(1), "Post 5", 5.5, categories[1]));
            posts.Add(new Post(DateTime.Now.AddYears(1).AddMonths(1), "Post 6", 6.6, categories[2]));

            posts.Add(new Post(DateTime.Now.AddYears(0).AddMonths(0), "Post 7", 7.7, categories[2]));
            posts.Add(new Post(DateTime.Now.AddYears(1).AddMonths(1), "Post 8", 8.8, categories[0]));
            posts.Add(new Post(DateTime.Now.AddYears(2).AddMonths(1), "Post 9", 9.9, categories[1]));

            profile.Accounts.Add(new Account("Account 1"));
            profile.Accounts[0].Posts.Add(posts[0]);
            profile.Accounts[0].Posts.Add(posts[1]);
            profile.Accounts[0].Posts.Add(posts[2]);
            profile.Accounts.Add(new Account("Account 2"));
            profile.Accounts[1].Posts.Add(posts[3]);
            profile.Accounts[1].Posts.Add(posts[4]);
            profile.Accounts[1].Posts.Add(posts[5]);
            profile.Accounts.Add(new Account("Account 3"));
            profile.Accounts[2].Posts.Add(posts[6]);
            profile.Accounts[2].Posts.Add(posts[7]);
            profile.Accounts[2].Posts.Add(posts[8]);

        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            CategorizeBarChart();
            CategorizeLineChart();
            CurrentNode = MasterNodesBarChart.First();
        }

        private void NodeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            Node n = e.AddedItems[0] as Node;
            if (n == null)
                return;

            CurrentNode = n;
        }

        private void ListBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb == null)
                return;

            Node n = lb.SelectedItem as Node;
            if (n == null)
                return;

            CurrentNode = n;
        }
    }
}
