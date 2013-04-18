using System;
using System.Collections.Generic;
using LunchViewer.Infrastructure;
using LunchViewer.Utils;
using Newtonsoft.Json;

namespace LunchViewer.Model
{
    public class DailyMenu : ObservableObject
    {
        public WeeklyMenu Parent { get; set; }
        public DateTime Date { get; set; }

        // Data in the currently selected language
        private string _Day;
        public string Day
        {
            get { return _Day; }
            set
            {
                if (value == _Day) return;
                _Day = value;
                NotifyPropertyChanged();
            }
        }

        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (value == _Text) return;
                _Text = value;
                NotifyPropertyChanged();
            }
        }

        // Cached translations (key = culture, value = translated text)
        [JsonPropertyAttribute]
        private readonly Dictionary<string, string> text_cache = new Dictionary<string, string>();
        
        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                if (value.Equals(_IsExpanded)) return;
                _IsExpanded = value;
                NotifyPropertyChanged();
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value.Equals(_IsSelected)) return;
                _IsSelected = value;
                NotifyPropertyChanged();
            }
        }

        public DailyMenu() : this(string.Empty, DateTime.Today, string.Empty) {}

        public DailyMenu(string culture, DateTime date, string text)
        {
            Date = date;
            AddNewTranslation(culture, text);

            IsExpanded = false;
            IsSelected = false;
        }

        public void Select()
        {
            IsSelected = true;
            Parent.IsExpanded = true;
        }

        public bool HasLanguage(string culture)
        {
            return text_cache.ContainsKey(culture);
        }

        public void SetLanguage(string culture)
        {
            if (!text_cache.ContainsKey(culture)) return;

            Day = DateUtils.GetDay(culture, Date);
            Text = text_cache[culture];
        }

        public string GetTranslation(string culture)
        {
            string str;
            text_cache.TryGetValue(culture, out str);
            return str;
        }

        public void AddNewTranslation(string culture, string text)
        {
            if (!string.IsNullOrEmpty(culture) && !text_cache.ContainsKey(culture))
                text_cache.Add(culture, text);
        }
    }
}
