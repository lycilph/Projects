using System.Collections.ObjectModel;
using System.Windows;

namespace LunchViewer
{
    public partial class NotificationWindow
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

        public void AddNotification(string text)
        {
            var fade_out_duration = ((Duration)Resources["fade_out_duration"]).TimeSpan;
            var message_duration = ((Duration)Resources["message_duration"]).TimeSpan;

            var notification = new Notification(text, Notifications.Remove, message_duration, fade_out_duration);
            Notifications.Insert(0, notification);
        }
    }
}
