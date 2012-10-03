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
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Threading;

namespace Wpf_tasks_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(MainWindow), new UIPropertyMetadata(string.Empty));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(MainWindow), new UIPropertyMetadata(false));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Status = "Ready";

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            if (timer == null)
                return;
            timer.Stop();

            Status = "Loading";
            IsBusy = true;

            var context = TaskScheduler.FromCurrentSynchronizationContext();

            Task task = Task.Factory.StartNew(
                () =>
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            // Update status
                            Task.Factory.StartNew(() => Status = string.Format("Working on iteration {0}", i), CancellationToken.None, TaskCreationOptions.None, context);
                            // Do fake work
                            Thread.Sleep(1000);
                        }
                    }
                )
                .ContinueWith(
                    _ =>
                        {
                            Status = "Ready";
                            IsBusy = false;
                        },
                    context
                );
        }
    }
}
