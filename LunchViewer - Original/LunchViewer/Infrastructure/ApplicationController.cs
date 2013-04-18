using System.ComponentModel.Composition;

namespace LunchViewer.Infrastructure
{
    [Export]
    public class ApplicationController
    {
        [Import]
        public IApplicationSettings Settings { get; set; }
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public ILocalizationService LocalizationService { get; set; }
        [Import]
        public INotificationService NotificationService { get; set; }
        [Import]
        public IMenuUpdateService MenuUpdateService { get; set; }
        [Import]
        public IMainWindow MainWindow { get; set; }
        [Import]
        public ITaskbarWindow TaskbarWindow { get; set; }

        public void Initialize()
        {
            // Load settings and data
            Settings = ApplicationSettings.Load();
            MenuRepository.Load();

            // Initialize controllers and services
            LocalizationService.Update(); // This is needed to set the initial language (found in settings)
            NotificationService.Start();
            MenuUpdateService.Start();

            //daily_reminder = new DailyReminder(settings, notification_service, menu_repository, menu_viewer);
            //daily_reminder.Start();
        }

        public void Terminate()
        {
            NotificationService.Stop();
            MenuUpdateService.Stop();

            // Save settings and data
            MenuRepository.Save();
            ApplicationSettings.Save(Settings);


            //daily_reminder.Stop();
            //menu_updater.StopCheckForUpdate();
        }
    }
}
