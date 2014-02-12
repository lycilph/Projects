using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace LoadImageAsync
{
    public partial class MainWindow
    {
        public ObservableCollection<AsyncImage> Images
        {
            get { return (ObservableCollection<AsyncImage>)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(ObservableCollection<AsyncImage>), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (s, e) => UpdateThreads();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Start();

            Images = new ObservableCollection<AsyncImage>();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var placeholder = new Uri(@"/LoadImageAsync;component/minus.png", UriKind.Relative);
            var image_source = new Uri(@"http://www.snap-undp.org/WeeklyHighlights/Lists/Photos/placeholder.png");
            //Images.Add(new AsyncImage(image_source, placeholder));

            Enumerable.Range(1, 3).Select(x =>
            {
                Images.Add(new AsyncImage(image_source, placeholder));
                return x;
            }).ToList();
        }

        private void UpdateThreads()
        {
            int availableThreads;
            int maxThreads;
            int availablePorts;
            int maxPorts;

            ThreadPool.GetAvailableThreads(out availableThreads, out availablePorts);
            ThreadPool.GetMaxThreads(out maxThreads, out maxPorts);

            status.Text = string.Format("Threads {0}-{1}={2}", maxThreads, availableThreads, (maxThreads - availableThreads));
        }
    }
}
