using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LunchViewer.Annotations;

namespace LunchViewer.Data
{
    public class WeekMenu : INotifyPropertyChanged
    {
        public string Header { get; set; }
        public int Week { get; set; }
        public string Link { get; set; }
        public ObservableCollection<DayMenu> Menus { get; set; }
        
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

        public WeekMenu() : this(string.Empty, string.Empty) {}

        public WeekMenu(string header, string link)
        {
            Header = header;
            Menus = new ObservableCollection<DayMenu>();

            var match = Regex.Match(Header, @"[\d].");
            if (match.Success)
                Week = Convert.ToInt32(match.Value);

            Link = link;
            IsExpanded = false;
            IsSelected = false;
        }

        public void Add(DayMenu day_menu)
        {
            day_menu.Parent = this;
            Menus.Add(day_menu);
        }

        public void Select()
        {
            IsSelected = true;
            IsExpanded = true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
