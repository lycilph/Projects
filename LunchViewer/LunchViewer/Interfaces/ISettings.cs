using System;
using System.ComponentModel;

namespace LunchViewer.Interfaces
{
    public interface ISettings
    {
        event PropertyChangedEventHandler PropertyChanged;

        void Load();
        void Save();

        void InitializeLanguage();

        string DefaultCulture { get; }
        string OriginalCulture { get; }

        bool AutomaticMenuUpdate { get; set; }
        bool EnableDailyReminder { get; set; }
        TimeSpan DailyReminder { get; set; }
        string ReminderEmail { get; set; }
        int UpdateInterval { get; set; }
        bool ShowNotificationOnUpdate { get; set; }
        string RepositoryPath { get; set; }
        string Culture { get; set; }
        bool TranslateMenus { get; set; }
        int NotificationDuration { get; set; }
    }
}