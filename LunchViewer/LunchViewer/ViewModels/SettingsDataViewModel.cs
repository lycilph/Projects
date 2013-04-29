using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Win32;

namespace LunchViewer.ViewModels
{
    [Export(typeof(SettingsDataViewModel))]
    public class SettingsDataViewModel : ObservableObject, IPartImportsSatisfiedNotification
    {
        [Import]
        private ISettings Settings { get; set; }
        [Import]
        private IMenuRepository MenuRepository { get; set; }
        [Import]
        private IMenuUpdateService MenuUpdateService { get; set; }
        [Import]
        private ILocalizationService LocalizationService { get; set; }
        [Import]
        private IDialogService DialogService { get; set; }

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

        public ICommand OpenFileDialogCommand { get; private set; }
        public ICommand CheckNowCommand { get; private set; }
        public ICommand ClearAllCommand { get; private set; }

        public SettingsDataViewModel()
        {
            OpenFileDialogCommand = new ActionCommand(ShowFileDialog);
            ClearAllCommand = new ActionCommand(ClearAll);
        }

        public void OnImportsSatisfied()
        {
            CheckNowCommand = new ActionCommand(MenuUpdateService.CheckNow);
        }

        private void ShowFileDialog()
        {
            var path = (string.IsNullOrWhiteSpace(Settings.RepositoryPath) ? Environment.CurrentDirectory : Settings.RepositoryPath);

            SaveFileDialog sdf = new SaveFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(path),
                FileName = Path.GetFileName(path),
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
