using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using LunchViewer.Data;
using LunchViewer.Utils;
using Newtonsoft.Json;

namespace LunchViewer.Infrastructure
{
    [Export(typeof(IMenuRepository))]
    public class MenuRepository : IMenuRepository
    {
        [Import]
        public IApplicationSettings Settings { get; set; }

        public ObservableCollection<WeekMenu> WeekMenus { get; set; }

        public MenuRepository()
        {
            WeekMenus = new ObservableCollection<WeekMenu>();
        }

        public bool HasMenuforWeek(int week)
        {
            return WeekMenus.Any(m => m.Week == week);
        }

        public WeekMenu GetMenuForWeek(int week)
        {
            return WeekMenus.First(m => m.Week == week);
        }

        public DayMenu GetTodaysMenu()
        {
            // This must be done using the da-DK culture (since kitenet is always in danish)
            var current_week = DateUtils.GetCurrentWeekNumber();
            var current_day = DateUtils.GetCurrentDay(CultureInfo.CreateSpecificCulture("da-DK")).ToLower();

            var week_menu = WeekMenus.FirstOrDefault(e => e.Week == current_week);
            return week_menu != null ? week_menu.Menus.FirstOrDefault(e => e.Day.ToLower().Contains(current_day)) : null;
        }

        public void Add(WeekMenu menu)
        {
            WeekMenus.Add(menu);
        }

        public void Load()
        {
            if (!File.Exists(Settings.RepositoryPath)) return;

            using (var fs = File.Open(Settings.RepositoryPath, FileMode.Open))
            using (var sw = new StreamReader(fs))
            {
                var json = sw.ReadToEnd();
                WeekMenus = JsonConvert.DeserializeObject<ObservableCollection<WeekMenu>>(json);
            }
        }

        public void Save()
        {
            using (var fs = File.Open(Settings.RepositoryPath, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var json = JsonConvert.SerializeObject(WeekMenus, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                sw.Write(json);
            }
        }
    }
}
