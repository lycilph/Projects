using System.Collections.ObjectModel;
using LunchViewer.Model;

namespace LunchViewer.Interfaces
{
    public interface IMenuRepository
    {
        ObservableCollection<WeeklyMenu> WeeklyMenus { get; set; }

        bool HasMenuforWeek(int week);
        WeeklyMenu GetMenuForWeek(int week);
        DailyMenu GetTodaysMenu();

        void Add(WeeklyMenu menu);
        void ClearAll();

        void Load();
        void Save();
    }
}
