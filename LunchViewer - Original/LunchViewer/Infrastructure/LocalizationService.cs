using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace LunchViewer.Infrastructure
{
    [Export(typeof(ILocalizationService))]
    public class LocalizationService : ILocalizationService, IPartImportsSatisfiedNotification
    {
        [Import]
        public IApplicationSettings Settings { get; set; }

        public void Update()
        {
            var culture_info = CultureInfo.CreateSpecificCulture(Settings.Culture);
            LocalizeDictionary.Instance.Culture = culture_info;
            NotifyLanguageChanged();
        }

        public string Translate(string key)
        {
            string str;
            LocExtension loc = new LocExtension("LunchViewer:Strings:" + key);
            loc.ResolveLocalizedValue(out str);
            return str;
        }

        public IEnumerable<string> GetAvailableCultures()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (path == null) return new List<string>();

            var resource_folders = Directory.EnumerateDirectories(path);
            return resource_folders.Select(Path.GetFileName);
        }

        public void OnImportsSatisfied()
        {
            Settings.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Culture")
                        Update();
                };
        }

        public event EventHandler LanguageChanged;

        protected virtual void NotifyLanguageChanged()
        {
            EventHandler handler = LanguageChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
