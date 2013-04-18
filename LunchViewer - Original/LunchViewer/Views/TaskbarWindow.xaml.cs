using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using LunchViewer.Annotations;
using LunchViewer.Infrastructure;
using Microsoft.Expression.Interactivity.Core;

namespace LunchViewer.Views
{
    [Export(typeof(ITaskbarWindow))]
    public partial class TaskbarWindow : ITaskbarWindow, IPartImportsSatisfiedNotification, INotifyPropertyChanged
    {
        [Import]
        public IMainWindow MainWindow { get; set; }
        [Import]
        public INotificationService NotificationService { get; set; }
        [Import]
        public IMenuRepository MenuRepository { get; set; }

        private ICommand _ShowCommand;
        public ICommand ShowCommand
        {
            get { return _ShowCommand; }
            private set
            {
                if (Equals(value, _ShowCommand)) return;
                _ShowCommand = value;
                NotifyPropertyChanged();
            }
        }

        public TaskbarWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Open();
        }

        private void OpenTodaysMenuClick(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenTodaysMenu();
        }

        private void ShowTodaysMenuClick(object sender, RoutedEventArgs e)
        {
            var todays_menu = MenuRepository.GetTodaysMenu();
            if (todays_menu != null)
                NotificationService.ShowNotification(todays_menu, () => MainWindow.Open(todays_menu));
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        public void OnImportsSatisfied()
        {
            ShowCommand = new ActionCommand(_ => MainWindow.Open());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
