using System;
using System.Windows;
using System.Windows.Input;

namespace AsyncTest
{
    public partial class MainWindow : Window
    {
        private readonly Engine engine = new Engine();

        public ICommand StartCommand { get; private set; }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MainWindow), new PropertyMetadata("Idle"));

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));

        public Visibility ProgressVisibility
        {
            get { return (Visibility)GetValue(ProgressVisibilityProperty); }
            set { SetValue(ProgressVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ProgressVisibilityProperty =
            DependencyProperty.Register("ProgressVisibility", typeof(Visibility), typeof(MainWindow), new PropertyMetadata(Visibility.Collapsed));

        public MainWindow()
        {
            InitializeComponent();

            StartCommand = new RelayCommand(_  => Start(), _ => ProgressVisibility == Visibility.Collapsed);

            DataContext = this;
        }

        private async void Start()
        {
            Text = "Working";
            Cursor = Cursors.Wait;
            ProgressVisibility = Visibility.Visible;

            var reporter = new Progress<double>(p => Progress = p);
            var result = await engine.DoStuffAsync(2000, reporter);

            ProgressVisibility = Visibility.Collapsed;
            Cursor = null;
            Text = string.Format("Done ({0})", result);

            // This must be called to make the Start command refresh properly
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
