using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LunchViewer.Annotations;
using LunchViewer.Data;
using LunchViewer.Infrastructure;
using LunchViewer.Utils;
using Microsoft.Expression.Interactivity.Core;

namespace LunchViewer.ViewModels
{
    [Export(typeof(MainViewModel))]
    public class MainViewModel : INotifyPropertyChanged, IPartImportsSatisfiedNotification
    {
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public ILocalizationService LocalizationService { get; set; }
        [Import]
        public SettingsViewModel SettingsViewModel { get; set; }

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

        public ICommand SettingsCommand { get; private set; }
        public ICommand LinkCommand { get; private set; }
        public ICommand TodayCommand { get; private set; }
        public ICommand ExpandAllCommand { get; private set; }
        public ICommand CollapseAllCommand { get; private set; }
        
        private void SetAllExpansion(bool is_expanded)
        {
            foreach (var week_menu in MenuRepository.WeekMenus)
                week_menu.IsExpanded = is_expanded;
        }

        private void SetAllSelection(bool is_selected)
        {
            foreach (var week_menu in MenuRepository.WeekMenus)
            {
                foreach (var daily_menu in week_menu.Menus)
                    daily_menu.IsSelected = is_selected;
                week_menu.IsSelected = is_selected;
            }
        }

        public void Show(WeekMenu week_menu)
        {
            SetAllExpansion(false);
            SetAllSelection(false);

            week_menu.Select();
        }

        public void Show(DayMenu day_menu)
        {
            if (day_menu == null) return;

            SetAllExpansion(false);
            SetAllSelection(false);

            day_menu.Select();
        }

        private string GetDateString()
        {
            var current_week = DateUtils.GetCurrentWeekNumber();
            var date = DateUtils.GetCurrentDateFormatted();
            var day = DateUtils.GetCurrentDay();

            var date_format = LocalizationService.Translate("DateHeader");

            return string.Format(date_format, day, date, current_week);
        }

        private void UpdateLanguage()
        {
            Date = GetDateString();
        }

        private void ShowLink(object link)
        {
            var url = link as string;
            if (url == null) return;

            Process.Start(url);
        }

        public void OnImportsSatisfied()
        {
            SettingsCommand = new ActionCommand(_ => SettingsViewModel.IsOpen = true);
            LinkCommand = new ActionCommand(ShowLink);
            TodayCommand = new ActionCommand(_ => Show(MenuRepository.GetTodaysMenu()));
            ExpandAllCommand = new ActionCommand(_ => SetAllExpansion(true));
            CollapseAllCommand = new ActionCommand(_ =>
            {
                SetAllSelection(false);
                SetAllExpansion(false);
            });

            LocalizationService.LanguageChanged += (sender, args) => UpdateLanguage();
            UpdateLanguage();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}