using System;
using System.ComponentModel;

namespace LunchViewer.Infrastructure
{
    public interface IApplicationSettings
    {
        event PropertyChangedEventHandler PropertyChanged;

        TimeSpan DailyReminder { get; set; }
        string RepositoryPath { get; set; }
        string Culture { get; set; }
    }
}