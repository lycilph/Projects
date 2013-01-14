using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;

namespace CategorizationEngine
{
    public class MainWindowViewModel : ObservableObject
    {
        private Profile profile;
        private CategoryController category_controller;
        private GraphDataGenerator graph_data_generator;
        private ScriptHost script_host;

        private ObservableCollection<Post> _Posts;
        public ObservableCollection<Post> Posts
        {
            get { return _Posts; }
            set
            {
                if (_Posts == value) return;
                _Posts = value;
                NotifyPropertyChanged("Posts");
            }
        }

        private Post _SelectedPost;
        public Post SelectedPost
        {
            get { return _SelectedPost; }
            set
            {
                if (_SelectedPost == value) return;
                _SelectedPost = value;
                NotifyPropertyChanged("SelectedPost");

                Root.SetPost(_SelectedPost);
            }
        }

        private CategoryViewModel _Root;
        public CategoryViewModel Root
        {
            get { return _Root; }
            set
            {
                if (_Root == value) return;
                _Root = value;
                NotifyPropertyChanged("Root");
            }
        }

        private ObservableCollection<Account> _Accounts;
        public ObservableCollection<Account> Accounts
        {
            get { return _Accounts; }
            set
            {
                if (_Accounts == value) return;
                _Accounts = value;
                NotifyPropertyChanged("Accounts");
            }
        }

        private ObservableCollection<KeyValuePair<string,double>> _GraphData;
        public ObservableCollection<KeyValuePair<string, double>> GraphData
        {
            get { return _GraphData; }
            set
            {
                if (_GraphData == value) return;
                _GraphData = value;
                NotifyPropertyChanged("GraphData");
            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title == value) return;
                _Title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string _Command;
        public string Command
        {
            get { return _Command; }
            set
            {
                if (_Command == value) return;
                _Command = value;
                NotifyPropertyChanged("Command");
            }
        }

        private string _Status;
        public string Status
        {
            get { return _Status; }
            set
            {
                if (_Status == value) return;
                _Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public ICommand ExecuteCommand { get; private set; }

        public MainWindowViewModel()
        {
            Initialize();

            ExecuteCommand = new RelayCommand(_ => Execute());

            Categorize();
            GenerateGraphData();
            RandomGraph();
        }

        private void Execute()
        {
            if (script_host.Execute(Command))
            {
                Command = string.Empty;
            }
        }

        public void Initialize()
        {
            profile = DataGenerator.GenerateProfile();
            category_controller = new CategoryController(profile);
            graph_data_generator = new GraphDataGenerator(profile);
            script_host = new ScriptHost(profile, category_controller, this);

            Posts = new ObservableCollection<Post>(profile.AggregatedPosts());
            Root = CategoryViewModel.Create(profile.RootCategory);
            Accounts = new ObservableCollection<Account>(profile.Accounts);
            Title = "Categorization engine [" + profile.Name + "]";

            SelectedPost = Posts.FirstOrDefault();
        }

        public void Categorize()
        {
            var elapsed_time = category_controller.Categorize();
            Status = "Elapsed (categorization): " + elapsed_time + " ms";

            // Update gui
            NotifyPropertyChanged("Posts");
            Root.SetPost(SelectedPost);
        }

        public void GenerateGraphData()
        {
            var elapsed_time = graph_data_generator.Execute();
            Status = "Elapsed (graph data): " + elapsed_time + " ms";
        }

        public void RandomGraph()
        {
            var sw = Stopwatch.StartNew();
            GraphData = new ObservableCollection<KeyValuePair<string, double>>();

            var rand = new Random();
            var time = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                var graph_point = new KeyValuePair<string, double>(time.ToShortDateString(), rand.NextDouble()*100 + 50);
                GraphData.Add(graph_point);
                time = time.AddMonths(1);
            }
            sw.Stop();
            Status = "Elapsed (graph): " + sw.ElapsedMilliseconds + " ms";
        }

        public void CategoryByYearGraph(Category category)
        {
            var sw = Stopwatch.StartNew();
            GraphData = new ObservableCollection<KeyValuePair<string, double>>(graph_data_generator.ByYear(category));
            sw.Stop();
            Status = "Elapsed (graph): " + sw.ElapsedMilliseconds + " ms";
        }
    }
}
