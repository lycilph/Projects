using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using LunchViewer.Data;
using LunchViewer.Infrastructure;

namespace LunchViewer.Views
{
    [Export(typeof(INotificationService))]
    public partial class NotificationWindow : INotificationService
    {
        public ObservableCollection<Notification> Notifications
        {
            get { return (ObservableCollection<Notification>)GetValue(NotificationsProperty); }
            set { SetValue(NotificationsProperty, value); }
        }
        public static readonly DependencyProperty NotificationsProperty =
            DependencyProperty.Register("Notifications", typeof(ObservableCollection<Notification>), typeof(MainWindow), new PropertyMetadata(null));

        public NotificationWindow()
        {
            InitializeComponent();
            DataContext = this;

            Notifications = new ObservableCollection<Notification>();

            var desktop_working_area = SystemParameters.WorkArea;
            Height = desktop_working_area.Height - 20;
            Left = desktop_working_area.Right - Width - 10;
            Top = desktop_working_area.Bottom - Height - 10;
        }

        public void Start()
        {
            Show();
        }

        public void Stop()
        {
            Close();
        }

        public void ShowNotification(object data)
        {
            Notify(data, delegate {});
        }

        public void ShowNotification(object data, Action click_action)
        {
            Notify(data, click_action);
        }

        private void Notify(object data, Action click_action)
        {
            var fade_out_duration = ((Duration) Resources["fade_out_duration"]).TimeSpan;
            var message_duration = ((Duration) Resources["message_duration"]).TimeSpan;

            var notification = new Notification(data, Notifications.Remove, click_action, message_duration, fade_out_duration);
            Notifications.Insert(0, notification);
        }
    }
}
