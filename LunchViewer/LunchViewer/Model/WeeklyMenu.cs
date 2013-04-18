using System.Collections.ObjectModel;
using LunchViewer.Infrastructure;

namespace LunchViewer.Model
{
    public class WeeklyMenu : ObservableObject
    {
        public ObservableCollection<DailyMenu> Menus { get; set; }

        public int Year { get; set; }
        public int Week { get; set; }

        public string Link { get; set; }

        private string _Header;
        public string Header
        {
            get { return _Header; }
            set
            {
                if (value == _Header) return;
                _Header = value;
                NotifyPropertyChanged();
            }
        }

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

        public WeeklyMenu() : this(0, 0, string.Empty) {}

        public WeeklyMenu(int year, int week, string link)
        {
            Menus = new ObservableCollection<DailyMenu>();
            Year = year;
            Week = week;
            Link = link;

            IsExpanded = false;
            IsSelected = false;
        }

        public void Add(DailyMenu dailyMenu)
        {
            dailyMenu.Parent = this;
            Menus.Add(dailyMenu);
        }

        public void Select()
        {
            IsSelected = true;
            IsExpanded = true;
        }

        public void SetLanguage(string translated_header)
        {
            Header = string.Format(translated_header, Week, Year);
        }
    }
}
