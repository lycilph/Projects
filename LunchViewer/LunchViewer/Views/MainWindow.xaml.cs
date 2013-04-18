using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LunchViewer.Interfaces;
using LunchViewer.Model;
using LunchViewer.ViewModels;

namespace LunchViewer.Views
{
    [Export(typeof(IMainWindow))]
    public partial class MainWindow : IMainWindow
    {
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public OverviewViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
        }

        public void Open()
        {
            if (Visibility == Visibility.Visible)
            {
                if (WindowState == WindowState.Minimized)
                    WindowState = WindowState.Normal;

                Activate();
            }
            else
                Show();
        }

        public void Open(WeeklyMenu week_menu)
        {
            ViewModel.Show(week_menu);
            Open();
        }

        public void Open(DailyMenu day_menu)
        {
            ViewModel.Show(day_menu);
            Open();
        }

        public void OpenTodaysMenu()
        {
            var todays_menu = MenuRepository.GetTodaysMenu();
            if (todays_menu != null)
                Open(todays_menu);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
