using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using LunchViewer.Model;
using LunchViewer.Utils;
using Microsoft.Expression.Interactivity.Core;

namespace LunchViewer.ViewModels
{
    [Export(typeof(OverviewViewModel))]
    public class OverviewViewModel : ObservableObject
    {
        private readonly DispatcherTimer timer;
        private DateTime today;

        [Import]
        public ISettings Settings { get; set; }
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public ILocalizationService LocalizationService { get; set; }

        private DailyMenu _CurrentMenu;
        public DailyMenu CurrentMenu
        {
            get { return _CurrentMenu; }
            set
            {
                if (Equals(value, _CurrentMenu)) return;
                _CurrentMenu = value;
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

        private string _Week;
        public string Week
        {
            get { return _Week; }
            private set
            {
                if (value == _Week) return;
                _Week = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ItemSelectionChangedCommand { get; private set; }
        public ICommand LinkCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }
        public ICommand TodayCommand { get; private set; }
        public ICommand ExpandAllCommand { get; private set; }
        public ICommand CollapseAllCommand { get; private set; }

        public OverviewViewModel()
        {
            today = DateTime.Today;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += (sender, args) =>
                {
                    if (DateTime.Today <= today) return;

                    today = DateTime.Today;
                    UpdateHeaders();
                };
            timer.Start();

            ItemSelectionChangedCommand = new ActionCommand(ItemSelectionChanged);
            LinkCommand = new ActionCommand(ShowLink);
            LoadedCommand = new ActionCommand(Loaded);
            TodayCommand = new ActionCommand(_ => Show(MenuRepository.GetTodaysMenu()));
            ExpandAllCommand = new ActionCommand(_ => SetAllExpansion(true));
            CollapseAllCommand = new ActionCommand(_ =>
                {
                    SetAllSelection(false);
                    SetAllExpansion(false);
                });
        }

        private void ItemSelectionChanged(object item)
        {
            var menu = item as DailyMenu;
            if (menu == null) return;

            CurrentMenu = menu;
        }

        private static void ShowLink(object link)
        {
            var url = link as string;
            if (url == null) return;

            Process.Start(url);
        }

        private void SetAllExpansion(bool is_expanded)
        {
            foreach (var weekly_menu in MenuRepository.WeeklyMenus)
                weekly_menu.IsExpanded = is_expanded;
        }

        private void SetAllSelection(bool is_selected)
        {
            foreach (var weekly_menu in MenuRepository.WeeklyMenus)
            {
                foreach (var daily_menu in weekly_menu.Menus)
                    daily_menu.IsSelected = is_selected;
                weekly_menu.IsSelected = is_selected;
            }
            SetCurrentMenu();
        }

        public void Show(WeeklyMenu weekly_menu)
        {
            SetAllExpansion(false);
            SetAllSelection(false);

            weekly_menu.Select();
        }

        public void Show(DailyMenu daily_menu)
        {
            if (daily_menu == null) return;

            SetAllExpansion(false);
            SetAllSelection(false);

            daily_menu.Select();
            ItemSelectionChanged(daily_menu);
        }

        public void Loaded()
        {
            UpdateHeaders();
            SetCurrentMenu();
        }

        private void SetCurrentMenu()
        {
            CurrentMenu = null;
            foreach (var weekly_menu in MenuRepository.WeeklyMenus)
                foreach (var daily_menu in weekly_menu.Menus)
                    if (daily_menu.IsSelected)
                        CurrentMenu = daily_menu;
        }

        private void UpdateHeaders()
        {
            // This function use the da-DK culture
            var current_week = DateUtils.GetCurrentWeekNumber();
            // This function use the currently selected culture
            var date = DateUtils.GetCurrentDateFormatted(Settings.Culture);
            // Localize
            var week_translation = LocalizationService.Localize("Week");

            Date = string.Format("{0}", date);
            Week = string.Format("{0} {1}", week_translation, current_week);
        }
    }
}
