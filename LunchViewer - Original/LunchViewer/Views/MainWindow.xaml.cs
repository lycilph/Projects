using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LunchViewer.Data;
using LunchViewer.Infrastructure;
using LunchViewer.ViewModels;

namespace LunchViewer.Views
{
    [Export(typeof(IMainWindow))]
    public partial class MainWindow : IMainWindow, IPartImportsSatisfiedNotification
    {
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public MainViewModel MainViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Open()
        {
            if (Visibility == Visibility.Visible)
                Activate();
            else
                Show();
        }

        public void Open(WeekMenu week_menu)
        {
            MainViewModel.Show(week_menu);
            Open();
        }

        public void Open(DayMenu day_menu)
        {
            MainViewModel.Show(day_menu);
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

        public void OnImportsSatisfied()
        {
            DataContext = MainViewModel;
        }
    }
}
