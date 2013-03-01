using System;
using System.Globalization;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using LunchViewer.Annotations;
using Microsoft.Expression.Interactivity.Core;

namespace LunchViewer
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<WeekMenu> _WeekMenus;
        public ObservableCollection<WeekMenu> WeekMenus
        {
            get { return _WeekMenus; }
            set
            {
                if (Equals(value, _WeekMenus)) return;
                _WeekMenus = value;
                NotifyPropertyChanged();
            }
        }

        private string _Date;
        public string Date
        {
            get { return _Date; }
            private set
            {
                if (value == _Date) return;
                _Date = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand TodayCommand { get; private set; }
        public ICommand ExpandAllCommand { get; private set; }
        public ICommand CollapseAllCommand { get; private set; }

        public MenuViewModel(ObservableCollection<WeekMenu> week_menus)
        {
            WeekMenus = week_menus;
            Date = GetDateString();

            TodayCommand = new ActionCommand(Today);
            ExpandAllCommand = new ActionCommand(e => SetAll(true));
            CollapseAllCommand = new ActionCommand(e => SetAll(false));

            var date_update_timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(5)};
            date_update_timer.Tick += (sender, args) => Date = GetDateString();
            date_update_timer.Start();
        }

        private void SetAll(bool is_expanded)
        {
            foreach (var week_menu in WeekMenus)
                week_menu.IsExpanded = is_expanded;
        }

        private void Today()
        {
            var current_week = DateUtils.GetCurrentWeekNumber();
            var current_day = DateUtils.GetCurrentDay(CultureInfo.CreateSpecificCulture("da-DA")).ToLower();

            foreach (var week_menu in WeekMenus.Where(wm => wm.Week == current_week))
            {
                week_menu.IsExpanded = true;
                foreach (var day_menu in week_menu.Menus.Where(dm => dm.Day.ToLower().Contains(current_day)))
                    day_menu.IsSelected = true;
            }
        }

        private static string GetDateString()
        {
            var current_week = DateUtils.GetCurrentWeekNumber();
            var date = DateUtils.GetCurrentDateFormatted();
            var day = DateUtils.GetCurrentDay();

            return string.Format("{0} {1} (week {2})", day, date, current_week);
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
