using System;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using LunchViewer.Interfaces;
using NLog;

namespace LunchViewer.Model
{
    [Export(typeof(IDailyReminderService))]
    public class DailyReminderService : IDailyReminderService, IPartImportsSatisfiedNotification
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly DispatcherTimer timer;

        [Import]
        private ISettings Settings { get; set; }
        [Import]
        private IMenuRepository MenuRepository { get; set; }
        [Import]
        private INotificationService NotificationService { get; set; }
        [Import]
        private IEmailService EmailService { get; set; }
        [Import]
        private IMainWindow MainWindow { get; set; }

        public DailyReminderService()
        {
            timer = new DispatcherTimer();
            timer.Tick += ReminderTick;
        }

        public void OnImportsSatisfied()
        {
            Settings.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "EnableDailyReminder")
                        UpdateStatus();
                    if (args.PropertyName == "DailyReminder")
                        SetTimeToNextReminder();
                };
        }

        public void Start()
        {
            logger.Debug("Enabling daily reminder");
            timer.Start();
        }

        public void Stop()
        {
            logger.Debug("Disabling daily reminder");
            timer.Stop();
        }

        private void UpdateStatus()
        {
            if (Settings.EnableDailyReminder)
                Start();
            else
                Stop();
        }

        private void ReminderTick(object sender, EventArgs e)
        {
            var todays_menu = MenuRepository.GetTodaysMenu();
            NotificationService.ShowNotification(todays_menu, () => MainWindow.Open(todays_menu));

            if (!string.IsNullOrWhiteSpace(Settings.ReminderEmail))
                EmailService.Send(todays_menu);

            SetTimeToNextReminder();
        }

        private void SetTimeToNextReminder()
        {
            var now = DateTime.Now;
            var dr = Settings.DailyReminder;
            var reminder_time = new DateTime(now.Year, now.Month, now.Day, dr.Hours, dr.Minutes, dr.Seconds);

            if (reminder_time < now)
                reminder_time = reminder_time.AddDays(1);

            logger.Debug("Setting daily reminder time to: " + reminder_time.ToLongTimeString());
            timer.Interval = reminder_time - now;
        }
    }
}
