using System.Collections.ObjectModel;
using LunchViewer.Data;

namespace LunchViewer.Infrastructure
{
    public interface IMenuRepository
    {
        ObservableCollection<WeekMenu> WeekMenus { get; set; }

        bool HasMenuforWeek(int week);
        WeekMenu GetMenuForWeek(int week);

        DayMenu GetTodaysMenu();

        void Add(WeekMenu menu);

        void Load();
        void Save();
    }
}
