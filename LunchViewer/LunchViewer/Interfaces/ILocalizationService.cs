using System.Collections.Generic;

namespace LunchViewer.Interfaces
{
    public interface ILocalizationService
    {
        string Localize(string key);
        string Localize(string key, string culture);
        IEnumerable<string> GetAvailableCultures();
    }
}