using LunchViewer.Model;

namespace LunchViewer.Interfaces
{
    public interface IMainWindow
    {
        void Open();
        void Open(WeeklyMenu week_menu);
        void Open(DailyMenu day_menu);
        void OpenTodaysMenu();
    }
}
