using System.ComponentModel.Composition;
using System.Windows;
using LunchViewer.Annotations;
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
     *   
     * 
     * For future user (if translation + storage is to be online)
     * AppFog (web server + db)
     * Email:    FirstLast_LunchViewer@outlook.com
     * Pasworkd: FirstLast123
     * Domain:   Lunchviewer.eu01.aws.af.cm
     * 
     * MongoHQ (test db)
     * Email:    FirstLast_LunchViewer@outlook.com
     * Pasworkd: FirstLast
     * 
     * Cloud9 (js ide)
     * Email:    FirstLast_LunchViewer@outlook.com
     * Pasworkd: FirstLast
     */

    public partial class App
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Import]
        private ISettings Settings { get; set; }
        [Import]
        private IMenuRepository MenuRepository { get; set; }
        [Import]
        private INotificationService NotificationService { get; set; }
        [Import]
        private IMenuUpdateService MenuUpdateService { [UsedImplicitly] get; set; }
        [Import]
        private IDailyReminderService DailyReminderService { [UsedImplicitly] get; set; }
        [Import]
        private ILocalizationService LocalizationService { [UsedImplicitly] get; set; }
        [Import]
        private ITranslationService TranslationService { [UsedImplicitly] get; set; }
        [Import]
        private IMainWindow Window { [UsedImplicitly] get; set; }
        [Import]
        private ITaskbarWindow TaskbarWindow { [UsedImplicitly] get; set; }

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
