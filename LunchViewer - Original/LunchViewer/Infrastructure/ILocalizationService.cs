using System;
using System.Collections.Generic;

namespace LunchViewer.Infrastructure
{
    public interface ILocalizationService
    {
        event EventHandler LanguageChanged;

        void Update();
        string Translate(string key);
        IEnumerable<string> GetAvailableCultures();
    }
}