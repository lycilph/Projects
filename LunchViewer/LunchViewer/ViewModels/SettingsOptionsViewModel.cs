using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;

namespace LunchViewer.ViewModels
{
    [Export(typeof(SettingsOptionsViewModel))]
    public class SettingsOptionsViewModel : ObservableObject, IPartImportsSatisfiedNotification
    {
        [Import]
        public ISettings Settings { get; set; }
        [Import]
        public ILocalizationService LocalizationService { get; set; }

        public string CurrentCulture
        {
            get { return Settings.Culture; }
            set
            {
                if (value == Settings.Culture) return;
                Settings.Culture = value;
                NotifyPropertyChanged();
            }
        }

        public bool TranslateMenus
        {
            get { return Settings.TranslateMenus; }
            set
            {
                if (value == Settings.TranslateMenus) return;
                Settings.TranslateMenus = value;
                NotifyPropertyChanged();
            }
        }

        // This is the update interval converted to minutes
        public int UpdateInterval
        {
            get { return Settings.UpdateInterval/60; }
            set 
            {
                var secs = value*60;
                if (secs == Settings.UpdateInterval) return;
                Settings.UpdateInterval = secs;
                NotifyPropertyChanged();
            }
        }

        public bool ShowNotificationOnUpdate
        {
            get { return Settings.ShowNotificationOnUpdate; }
            set
            {
                if (value == Settings.ShowNotificationOnUpdate) return;
                Settings.ShowNotificationOnUpdate = value;
                NotifyPropertyChanged();
            }
        }

        public bool AutomaticUpdate
        {
            get { return Settings.AutomaticMenuUpdate; }
            set
            {
                if (value == Settings.AutomaticMenuUpdate) return;
                Settings.AutomaticMenuUpdate = value;
                NotifyPropertyChanged();
            }
        }

        public bool EnableDailyReminder
        {
            get { return Settings.EnableDailyReminder; }
            set
            {
                if (value == Settings.EnableDailyReminder) return;
                Settings.EnableDailyReminder = value;
                NotifyPropertyChanged();
            }
        }

        public TimeSpan DailyReminder
        {
            get { return Settings.DailyReminder; }
            set
            {
                if (value == Settings.DailyReminder) return;
                Settings.DailyReminder = value;
                NotifyPropertyChanged();
            }
        }

        public string ReminderEmail
        {
            get { return Settings.ReminderEmail; }
            set
            {
                if (value == Settings.ReminderEmail) return;
                Settings.ReminderEmail = value;
                NotifyPropertyChanged();
            }
        }

        public int NotificationDuration
        {
            get { return Settings.NotificationDuration; }
            set
            {
                if (value == Settings.NotificationDuration) return;
                Settings.NotificationDuration = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<string> Cultures { get; private set; }

        public void OnImportsSatisfied()
        {
            Cultures = LocalizationService.GetAvailableCultures();

            Settings.PropertyChanged += (sender, args) => NotifyPropertyChanged(string.Empty);
        }
    }
}
