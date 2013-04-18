using LunchViewer.Data;

namespace LunchViewer.Infrastructure
{
    public interface IMainWindow
    {
        void Open();
        void Open(WeekMenu week_menu);
        void Open(DayMenu day_menu);
        void OpenTodaysMenu();
    }
}
