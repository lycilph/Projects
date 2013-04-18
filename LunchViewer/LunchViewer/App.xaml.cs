using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using NLog;

namespace LunchViewer
{
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
            logger.Debug("Starting services");
            NotificationService.Start();
            MenuUpdateService.Start();
            DailyReminderService.Start();

            // Initialize language
            Settings.InitializeLanguage();
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            // Stop services
            logger.Debug("Stopping services");
            NotificationService.Stop();
            MenuUpdateService.Stop();
            DailyReminderService.Stop();

            // Save settings and data
            logger.Debug("Saving settings and data");
            Settings.Save();
            MenuRepository.Save();

            logger.Debug("Application stopped");
        }
    }
}
