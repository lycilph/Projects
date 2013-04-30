using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using NLog;
using Newtonsoft.Json;

namespace LunchViewer.Model
{
    [Export(typeof(ISettings))]
    public class Settings : ObservableObject, ISettings
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private const string app_folder = "LunchViewer";
        private const string settings_filename = "settings.json";
        private const string default_repository_filename = "menu_repository.json";
        private const string default_culture = "en-US";
        private const string original_culture = "da-DK";

        [JsonIgnore]
        public string DefaultCulture
        {
            get { return default_culture; }
        }

        [JsonIgnore]
        public string OriginalCulture
        {
            get { return original_culture; }
        }

        private bool _AutomaticMenuUpdate;
        public bool AutomaticMenuUpdate
        {
            get { return _AutomaticMenuUpdate; }
            set
            {
                if (value.Equals(_AutomaticMenuUpdate)) return;
                _AutomaticMenuUpdate = value;
                NotifyPropertyChanged();
            }
        }

        private bool _EnableDailyReminder;
        public bool EnableDailyReminder
        {
            get { return _EnableDailyReminder; }
            set
            {
                if (value.Equals(_EnableDailyReminder)) return;
                _EnableDailyReminder = value;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan _DailyReminder;
        public TimeSpan DailyReminder
        {
            get { return _DailyReminder; }
            set
            {
                if (_DailyReminder == value) return;
                _DailyReminder = value;
                NotifyPropertyChanged();
            }
        }

        private string _ReminderEmail;
        public string ReminderEmail
        {
            get { return _ReminderEmail; }
            set
            {
                if (value == _ReminderEmail) return;
                _ReminderEmail = value;
                NotifyPropertyChanged();
            }
        }

        // This is an interval in seconds between menu updates
        private int _UpdateInterval;
        public int UpdateInterval
        {
            get { return _UpdateInterval; }
            set
            {
                if (value == _UpdateInterval) return;
                _UpdateInterval = value;
                NotifyPropertyChanged();
            }
        }

        private bool _ShowNotificationOnUpdate;
        public bool ShowNotificationOnUpdate
        {
            get { return _ShowNotificationOnUpdate; }
            set
            {
                if (value.Equals(_ShowNotificationOnUpdate)) return;
                _ShowNotificationOnUpdate = value;
                NotifyPropertyChanged();
            }
        }

        private string _RepositoryPath;
        public string RepositoryPath
        {
            get { return _RepositoryPath; }
            set
            {
                if (_RepositoryPath == value) return;
                _RepositoryPath = value;
                NotifyPropertyChanged();
            }
        }

        private string _Culture;
        public string Culture
        {
            get { return _Culture; }
            set
            {
                if (_Culture == value) return;
                _Culture = value;
                NotifyPropertyChanged();
            }
        }

        private bool _TranslateMenus;
        public bool TranslateMenus
        {
            get { return _TranslateMenus; }
            set
            {
                if (value.Equals(_TranslateMenus)) return;
                _TranslateMenus = value;
                NotifyPropertyChanged();
            }
        }

        // This is the duration (in seconds) a notification is visible
        private int _NotificationDuration;
        public int NotificationDuration
        {
            get { return _NotificationDuration; }
            set
            {
                if (value == _NotificationDuration) return;
                _NotificationDuration = value;
                NotifyPropertyChanged();
            }
        }

        private bool _StartOnWindowsStart;
        public bool StartOnWindowsStart
        {
            get { return _StartOnWindowsStart; }
            set
            {
                if (value.Equals(_StartOnWindowsStart)) return;
                _StartOnWindowsStart = value;
                NotifyPropertyChanged();
                // Make sure the startup link is set correctly
                UpdateStartupLink();
            }
        }

        public Settings()
        {
            AutomaticMenuUpdate = true;
            EnableDailyReminder = true;
            DailyReminder = new TimeSpan(12, 0, 0);
            ReminderEmail = string.Empty;
            UpdateInterval = 60;
            ShowNotificationOnUpdate = true;
            RepositoryPath = Path.Combine(app_data_path, app_folder, default_repository_filename);
            Culture = default_culture;
            TranslateMenus = true;
            NotificationDuration = 6;
            StartOnWindowsStart = true;

            // Make sure the folder exists - does nothing if folder already exists
            Directory.CreateDirectory(app_data_path);
            // Set application startup link
            UpdateStartupLink();
        }

        public void Load()
        {
            logger.Debug("Trying to load settings");

            var settings_path = Path.Combine(app_data_path, app_folder, settings_filename);
            if (!File.Exists(settings_path)) return;

            logger.Debug("Found settings file: " + settings_path);

            using (var fs = File.Open(settings_path, FileMode.Open))
            using (var sw = new StreamReader(fs))
            {
                var json = sw.ReadToEnd();
                JsonConvert.PopulateObject(json, this);
            }
        }

        public void Save()
        {
            var settings_path = Path.Combine(app_data_path, app_folder, settings_filename);

            logger.Debug("Saving settings to: " + settings_path);

            using (var fs = File.Open(settings_path, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var json = JsonConvert.SerializeObject(this, Formatting.Indented);
                sw.Write(json);
            }
        }

        public void InitializeLanguage()
        {
            NotifyPropertyChanged("Culture");
        }

        private void UpdateStartupLink()
        {
            var app_path = Assembly.GetExecutingAssembly().Location;
            var app_filename = Path.GetFileName(app_path);
            Debug.Assert(app_filename != null, "app_filename != null");
            var link_filename = Path.ChangeExtension(app_filename, "lnk");

            var link_path = Path.ChangeExtension(app_path, "lnk");
            var startup_folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var startup_link_path = Path.Combine(startup_folder, link_filename);

            if (StartOnWindowsStart && !File.Exists(startup_link_path))
                File.Copy(link_path, startup_link_path, true);
            else if (!StartOnWindowsStart && File.Exists(startup_link_path))
                File.Delete(startup_link_path);
            // else nothing to do
        }
    }
}
