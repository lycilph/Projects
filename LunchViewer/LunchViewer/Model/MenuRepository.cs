using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using LunchViewer.Interfaces;
using LunchViewer.Utils;
using NLog;
using Newtonsoft.Json;

namespace LunchViewer.Model
{
    [Export(typeof(IMenuRepository))]
    public class MenuRepository : IMenuRepository
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Import]
        private ISettings Settings { get; set; }

        public ObservableCollection<WeeklyMenu> WeeklyMenus { get; set; }

        public MenuRepository()
        {
            WeeklyMenus = new ObservableCollection<WeeklyMenu>();
        }

        public bool HasMenuforWeek(int week)
        {
            return WeeklyMenus.Any(m => m.Week == week);
        }

        public WeeklyMenu GetMenuForWeek(int week)
        {
            return WeeklyMenus.First(m => m.Week == week);
        }

        public DailyMenu GetTodaysMenu()
        {
            // Get current week
            var current_week = DateUtils.GetCurrentWeekNumber();
            var today = DateTime.Today;

            var week_menu = WeeklyMenus.FirstOrDefault(e => e.Year == today.Year && e.Week == current_week);
            return week_menu != null ? week_menu.Menus.FirstOrDefault(e => e.Date.Day == today.Day) : null;
        }

        public void Add(WeeklyMenu menu)
        {
            logger.Debug(string.Format("Adding weekly menu - week {0}, {1}", menu.Week, menu.Year));

            WeeklyMenus.Add(menu);
        }

        public void ClearAll()
        {
            logger.Debug("Clearing menus");

            WeeklyMenus.Clear();
        }

        public void Load()
        {
            logger.Debug("Trying to load repository");

            if (!File.Exists(Settings.RepositoryPath)) return;

            logger.Debug("Found repository file: " + Settings.RepositoryPath);

            using (var fs = File.Open(Settings.RepositoryPath, FileMode.Open))
            using (var sw = new StreamReader(fs))
            {
                var json = sw.ReadToEnd();
                WeeklyMenus = JsonConvert.DeserializeObject<ObservableCollection<WeeklyMenu>>(json);
            }
        }

        public void Save()
        {
            logger.Debug("Saving repository to: " + Settings.RepositoryPath);

            using (var fs = File.Open(Settings.RepositoryPath, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var json = JsonConvert.SerializeObject(WeeklyMenus, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                sw.Write(json);
            }
        }
    }
}
