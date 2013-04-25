using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using LunchViewer.Model;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Win32;

namespace LunchViewer.ViewModels
{
    [Export(typeof(SettingsDataViewModel))]
    public class SettingsDataViewModel : ObservableObject, IPartImportsSatisfiedNotification
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
