using System;
using System.ComponentModel.Composition;
using System.IO;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using Newtonsoft.Json;

namespace LunchViewer.Model
{
    [Export(typeof(ISettings))]
    public class Settings : ObservableObject, ISettings
    {
        private static readonly string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private const string app_folder = "LunchViewer";
        private const string settings_filename = "settings.json";
        private const string default_repository_filename = "menu_repository.json";
        private const string default_culture = "en-US";
        private const string original_culture = "da-DK";

        public string DefaultCulture
        {
            get { return default_culture; }
        }

        public string OriginalCulture
        {
            get { return original_culture; }
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

        public Settings()
        {
            DailyReminder = new TimeSpan(12, 0, 0);
            UpdateInterval = 60;
            ShowNotificationOnUpdate = true;
            RepositoryPath = Path.Combine(app_data_path, app_folder, default_repository_filename);
            Culture = default_culture;
            TranslateMenus = true;

            // Make sure the folder exists - does nothing if folder already exists
            Directory.CreateDirectory(app_data_path);
        }

        public void Load()
        {
            var settings_path = Path.Combine(app_data_path, app_folder, settings_filename);
            if (!File.Exists(settings_path)) return;

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
    }
}
