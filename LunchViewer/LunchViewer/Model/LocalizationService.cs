using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using LunchViewer.Interfaces;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace LunchViewer.Model
{
    [Export(typeof(ILocalizationService))]
    public class LocalizationService : ILocalizationService, IPartImportsSatisfiedNotification
    {
        [Import]
        public ISettings Settings { get; set; }

        public void Update()
        {
            var culture_info = CultureInfo.CreateSpecificCulture(Settings.Culture);
            LocalizeDictionary.Instance.Culture = culture_info;
        }

        public string Localize(string key)
        {
            string str;
            LocExtension loc = new LocExtension("LunchViewer:Strings:" + key);
            loc.ResolveLocalizedValue(out str);
            return str;
        }

        public string Localize(string key, string culture)
        {
            var culture_info = CultureInfo.CreateSpecificCulture(culture);

            string str;
            LocExtension loc = new LocExtension("LunchViewer:Strings:" + key);
            loc.ResolveLocalizedValue(out str, culture_info);
            return str;
        }

        public IEnumerable<string> GetAvailableCultures()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (path == null) return new List<string>();

            var folders = Directory.EnumerateDirectories(path);
            var resource_folders = folders.Where(folder => (Directory.EnumerateFiles(folder).Any(file => file.EndsWith("resources.dll"))));
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
    }
}
