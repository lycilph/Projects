using System;

namespace LunchViewer.Interfaces
{
    public interface IMenuUpdateService
    {
        event EventHandler MenusUpdated;

        void Start();
        void Stop();

        void CheckNow();
    }
}
