using System;
using System.Windows.Threading;
using LunchViewer.Infrastructure;

namespace LunchViewer
{
    //public class DailyReminder : IDailyReminder
    //{
    //    private readonly ApplicationSettings _applicationSettings;
    //    private readonly INotificationService notification_service;
    //    private readonly IMenuRepository menu_repository;
    //    private readonly IMainWindow menu_viewer;
    //    private readonly DispatcherTimer timer;

    //    public DailyReminder(ApplicationSettings _applicationSettings, INotificationService notification_service, IMenuRepository menu_repository, IMainWindow menu_viewer)
    //    {
    //        this._applicationSettings = _applicationSettings;
    //        this.notification_service = notification_service;
    //        this.menu_repository = menu_repository;
    //        this.menu_viewer = menu_viewer;

    //        _applicationSettings.SettingsChanged += (sender, args) => Start();

    //        timer = new DispatcherTimer();
    //        timer.Tick += ReminderTick;
    //    }

    //    private void ReminderTick(object sender, EventArgs e)
    //    {
    //        var todays_menu = menu_repository.GetTodaysMenu();
    //        notification_service.ShowNotification(todays_menu, () => menu_viewer.Show(todays_menu));

    //        Start();
    //    }

    //    public void Start()
    //    {
    //        SetTimeToNextReminder();
    //        timer.Start();
    //    }

    //    public void Stop()
    //    {
    //        timer.Stop();
    //    }

    //    private void SetTimeToNextReminder()
    //    {
    //        var now = DateTime.Now;
    //        var dr = _applicationSettings.DailyReminder;
    //        var reminder_time = new DateTime(now.Year, now.Month, now.Day, dr.Hours, dr.Minutes, dr.Seconds);
            
    //        if (reminder_time < now)
    //            reminder_time = reminder_time.AddDays(1);

    //        timer.Interval = reminder_time - now;
    //    }
    //}
}