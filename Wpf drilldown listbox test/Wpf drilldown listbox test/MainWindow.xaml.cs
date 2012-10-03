using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Module_Interface;

namespace Wpf_drilldown_listbox_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<string> themes;

        public string CurrentTheme
        {
            get { return (string)GetValue(CurrentThemeProperty); }
            set { SetValue(CurrentThemeProperty, value); }
        }
        public static readonly DependencyProperty CurrentThemeProperty =
            DependencyProperty.Register("CurrentTheme", typeof(string), typeof(MainWindow), new UIPropertyMetadata(null));

        public Node MasterNode
        {
            get { return (Node)GetValue(MasterNodeProperty); }
            set { SetValue(MasterNodeProperty, value); }
        }
        public static readonly DependencyProperty MasterNodeProperty =
            DependencyProperty.Register("MasterNode", typeof(Node), typeof(MainWindow), new UIPropertyMetadata(null));

        public ObservableCollection<Node> BreadCrumbs
        {
            get { return (ObservableCollection<Node>)GetValue(BreadCrumbsProperty); }
            set { SetValue(BreadCrumbsProperty, value); }
        }
        public static readonly DependencyProperty BreadCrumbsProperty =
            DependencyProperty.Register("BreadCrumbs", typeof(ObservableCollection<Node>), typeof(MainWindow), new UIPropertyMetadata(null));

        public ICommand NavigateToBreadCrumbCommand { get; private set; }
        public ICommand DrillDownCommand { get; private set; }
        public ICommand ChangeThemeCommand { get; private set; }

        public ICommand StartNodeCommand { get; private set; }
        public ICommand BackNodeCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            #region MasterData generation
            Node root = new Node("Root");
            root.Nodes.Add(new Node("Level 1 - item 1"));
            root.Nodes.Add(new Node("Level 1 - item 2"));
            root.Nodes.Add(new Node("Level 1 - item 3"));
            root.Nodes.Add(new Node("Level 1 - item 4"));
            root.Nodes.Add(new Node("Level 1 - item 5"));

            Node d = root.Nodes.Skip(1).First();
            d.Nodes.Add(new Node("Level 2 - item 1"));
            d.Nodes.Add(new Node("Level 2 - item 2"));
            d.Nodes.Add(new Node("Level 2 - item 3"));
            d.Nodes.Add(new Node("Level 2 - item 4"));

            d = d.Nodes.Skip(2).First();
            d.Nodes.Add(new Node("Level 3 - item 1"));
            d.Nodes.Add(new Node("Level 3 - item 2"));
            d.Nodes.Add(new Node("Level 3 - item 3")); 
            #endregion

            #region Themes generation
            themes = new List<string> { "ExpressionDark", "ExpressionLight", "ShinyBlue", "ShinyRed" };
            CurrentTheme = themes.First();
            #endregion

            NavigateToBreadCrumbCommand = new RelayCommand(NavigateToBreadCrumb);
            DrillDownCommand = new RelayCommand(DrillDown, CanDrillDown);
            ChangeThemeCommand = new RelayCommand(ChangeTheme);

            StartNodeCommand = new RelayCommand(StartNode, _ => BreadCrumbs.Count > 1);
            BackNodeCommand = new RelayCommand(BackNode, _ => BreadCrumbs.Count > 1);

            BreadCrumbs = new ObservableCollection<Node>();
            BreadCrumbs.CollectionChanged += BreadCrumbsCollectionChanged;
            BreadCrumbs.Add(root);
        }

        private void NavigateToBreadCrumb(object obj)
        {
            Node n = obj as Node;
            if (n == null)
                return;

            while (BreadCrumbs.Count > 0 && BreadCrumbs.Last() != n)
                BreadCrumbs.Remove(BreadCrumbs.Last());
        }

        private void DrillDown(object obj)
        {
            Node n = obj as Node;
            if (n == null)
                return;

            BreadCrumbs.Add(n);
        }

        private bool CanDrillDown(object obj)
        {
            Node n = obj as Node;
            if (n == null)
                return false;

            return n.Nodes.Count > 0;
        }

        private void ChangeTheme(object obj)
        {
            // Remove all current theme resources
            var theme_resources = Application.Current.Resources.MergedDictionaries.Where(res => res is ThemeResourceDictionary).ToList();
            foreach (var theme in theme_resources)
                Application.Current.Resources.MergedDictionaries.Remove(theme);

            int current_theme_index = themes.IndexOf(CurrentTheme);
            int new_theme_index = (current_theme_index + 1 == themes.Count ? 0 : current_theme_index + 1);

            CurrentTheme = themes[new_theme_index];

            // Add a new theme
            Application.Current.Resources.MergedDictionaries.Add(new ThemeResourceDictionary() { Source = new Uri(@"/Themes/" + CurrentTheme + ".xaml", UriKind.Relative) });
            Application.Current.Resources.MergedDictionaries.Add(new ThemeResourceDictionary() { Source = new Uri(@"/Themes/" + CurrentTheme + "Extra.xaml", UriKind.Relative) });
        }

        void BreadCrumbsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            MasterNode = BreadCrumbs.Last();
        }

        private void StartNode(object obj)
        {
            while (BreadCrumbs.Count > 1)
                BreadCrumbs.Remove(BreadCrumbs.Last());
        }

        private void BackNode(object obj)
        {
            if (BreadCrumbs.Count > 1)
                BreadCrumbs.Remove(BreadCrumbs.Last());
        }
    }
}
