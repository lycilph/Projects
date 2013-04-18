using System;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using LunchViewer.Interfaces;

namespace LunchViewer.Model
{
    [Export(typeof(IDailyReminderService))]
    public class DailyReminderService : IDailyReminderService
    {
        private readonly DispatcherTimer timer;

        [Import]
        public ISettings Settings { get; set; }
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public INotificationService NotificationService { get; set; }
        [Import]
        public IMainWindow MainWindow { get; set; }

        public DailyReminderService()
        {
            timer = new DispatcherTimer();
            timer.Tick += ReminderTick;
        }

        public void Start()
        {
            SetTimeToNextReminder();
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void ReminderTick(object sender, EventArgs e)
        {
            var todays_menu = MenuRepository.GetTodaysMenu();
            NotificationService.ShowNotification(todays_menu, () => MainWindow.Open(todays_menu));

            Start();
        }

        private void SetTimeToNextReminder()
        {
            var now = DateTime.Now;
            var dr = Settings.DailyReminder;
            var reminder_time = new DateTime(now.Year, now.Month, now.Day, dr.Hours, dr.Minutes, dr.Seconds);

            if (reminder_time < now)
                reminder_time = reminder_time.AddDays(1);

            timer.Interval = reminder_time - now;
        }
    }
}
