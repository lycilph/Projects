using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using LunchViewer.Annotations;
using Newtonsoft.Json;

namespace LunchViewer.Infrastructure
{
    [Export(typeof(IApplicationSettings))]
    public class ApplicationSettings : IApplicationSettings, INotifyPropertyChanged
    {
        private static readonly string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private const string app_folder = "LunchViewer";
        private const string settings_filename = "settings.json";
        private const string default_repository_filename = "menu_repository.json";
        private const string default_culture = "en-US";

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

        public ApplicationSettings()
        {
            DailyReminder = new TimeSpan(12, 0, 0);
            RepositoryPath = Path.Combine(app_data_path, app_folder, default_repository_filename);
            Culture = default_culture;

            // Make sure the folder exists - does nothing if folder already exists
            Directory.CreateDirectory(app_data_path);
        }

        public static ApplicationSettings Load()
        {
            var settings_path = Path.Combine(app_data_path, app_folder, settings_filename);

            if (!File.Exists(settings_path))
                return new ApplicationSettings();

            using (var fs = File.Open(settings_path, FileMode.Open))
            using (var sw = new StreamReader(fs))
            {
                var json = sw.ReadToEnd();
                return JsonConvert.DeserializeObject<ApplicationSettings>(json);
            }
        }

        public static void Save(IApplicationSettings application_settings)
        {
            var settings = application_settings as ApplicationSettings;
            if (settings == null)
                throw new ArgumentException("Settings must be of type ApplicationSettings");
            Save(settings);
        }

        public static void Save(ApplicationSettings application_settings)
        {
            var settings_path = Path.Combine(app_data_path, app_folder, settings_filename);

            using (var fs = File.Open(settings_path, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var json = JsonConvert.SerializeObject(application_settings, Formatting.Indented);
                sw.Write(json);
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
