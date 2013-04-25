using LunchViewer.Model;

namespace LunchViewer.Interfaces
{
    public interface IEmailService
    {
        void Send(DailyMenu daily_menu);
    }
}
