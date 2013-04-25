using System.ComponentModel.Composition;
using System.Windows;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using NLog;

namespace LunchViewer
{
    /* Live account:
     * EMail:             FirstLast_LunchViewer@outlook.com
     * Password:          FirstLast
     * Security question: Name of first pet = mypet
     * 
     * Azuremarket:
     * First name: FirstName
     * Last name:  LastName
     * EMail:      FirstLast_LunchViewer@outlook.com
     *   Application:
     *   Client ID:     LunchViewerID
     *   Name:          LunchViewer
     *   Client secret: 9J9NlGfKUUU4j4UFNmbDUHz/oV2bQXBATevKDix8f/8=
     */


    public partial class App
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Import]
        public ISettings Settings { get; set; }
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public INotificationService NotificationService { get; set; }
        [Import]
        public IMenuUpdateService MenuUpdateService { get; set; }
        [Import]
        public IDailyReminderService DailyReminderService { get; set; }
        [Import]
        public ILocalizationService LocalizationService { get; set; }
        [Import]
        public ITranslationService TranslationService { get; set; }
        [Import]
        public IMainWindow Window { get; set; }
        [Import]
        public ITaskbarWindow TaskbarWindow { get; set; }

        [Import]
        private IEmailService EmailService { get; set; }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            logger.Debug("Application started");

            logger.Debug("Composing application");
            CompositionService.Compose(this);

            // Load settings and data
            logger.Debug("Loading settings and data");
            Settings.Load();
            MenuRepository.Load();

            // Start services
            logger.Debug("Starting service(s)");
            NotificationService.Start();

            // Initialize language
            logger.Debug("Initializing language");
            Settings.InitializeLanguage();

            EmailService.Send(MenuRepository.GetTodaysMenu());
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            // Stop services
            logger.Debug("Stopping service(s)");
            NotificationService.Stop();

            // Save settings and data
            logger.Debug("Saving settings and data");
            Settings.Save();
            MenuRepository.Save();

            logger.Debug("Application stopped");
        }
    }
}
