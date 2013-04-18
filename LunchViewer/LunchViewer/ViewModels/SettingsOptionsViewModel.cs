using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Win32;

namespace LunchViewer.ViewModels
{
    [Export(typeof(SettingsOptionsViewModel))]
    public class SettingsOptionsViewModel : ObservableObject, IPartImportsSatisfiedNotification
    {
        [Import]
        public ISettings Settings { get; set; }
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public IMenuUpdateService MenuUpdateService { get; set; }
        [Import]
        public ILocalizationService LocalizationService { get; set; }
        [Import]
        public IDialogService DialogService { get; set; }

        public string CurrentCulture
        {
            get { return Settings.Culture; }
            set
            {
                if (value == Settings.Culture) return;
                Settings.Culture = value;
                NotifyPropertyChanged();
            }
        }

        public bool TranslateMenus
        {
            get { return Settings.TranslateMenus; }
            set
            {
                if (value == Settings.TranslateMenus) return;
                Settings.TranslateMenus = value;
                NotifyPropertyChanged();
            }
        }

        // This is the update interval converted to minutes
        public int UpdateInterval
        {
            get { return Settings.UpdateInterval/60; }
            set 
            {
                var secs = value*60;
                if (secs == Settings.UpdateInterval) return;
                Settings.UpdateInterval = secs;
                NotifyPropertyChanged();
            }
        }

        public bool ShowNotificationOnUpdate
        {
            get { return Settings.ShowNotificationOnUpdate; }
            set
            {
                if (value == Settings.ShowNotificationOnUpdate) return;
                Settings.ShowNotificationOnUpdate = value;
                NotifyPropertyChanged();
            }
        }

        public string RepositoryPath
        {
            get { return Settings.RepositoryPath; }
            set
            {
                if (value == Settings.RepositoryPath) return;
                Settings.RepositoryPath = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<string> Cultures { get; private set; }

        public ICommand OpenFileDialogCommand { get; private set; }
        public ICommand CheckNowCommand { get; private set; }
        public ICommand ClearAllCommand { get; private set; }

        public SettingsOptionsViewModel()
        {
            OpenFileDialogCommand = new ActionCommand(ShowFileDialog);
            ClearAllCommand = new ActionCommand(ClearAll);
        }

        public void OnImportsSatisfied()
        {
            Cultures = LocalizationService.GetAvailableCultures();
            CheckNowCommand = new ActionCommand(MenuUpdateService.CheckNow);
        }

        private void ShowFileDialog()
        {
            SaveFileDialog sdf = new SaveFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(RepositoryPath),
                    FileName = Path.GetFileName(RepositoryPath),
                    DefaultExt = ".json",
                    Filter = "Repository files (.json)|*json",
                    AddExtension = true
                };

            // Show open file dialog box
            var result = sdf.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
                RepositoryPath = sdf.FileName;
        }

        private void ClearAll()
        {
            var title = LocalizationService.Localize("Warning");
            var message = LocalizationService.Localize("ClearAllWarning");
            var result = DialogService.ShowYesNoMessage(message, title);

            if (result == true)
                MenuRepository.ClearAll();
        }
    }
}
