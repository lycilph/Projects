using System;

namespace LunchViewer.Interfaces
{
    public interface INotificationService
    {
        void Start();
        void Stop();

        void ShowNotification(object data);
        void ShowNotification(object data, Action click_action);
    }
}
