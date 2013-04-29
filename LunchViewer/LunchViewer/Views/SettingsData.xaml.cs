using System.ComponentModel.Composition;
using LunchViewer.ViewModels;

namespace LunchViewer.Views
{
    public partial class SettingsData : IPartImportsSatisfiedNotification
    {
        [Import]
        private SettingsDataViewModel ViewModel { get; set; }

        public SettingsData()
        {
            InitializeComponent();
        }

        public void OnImportsSatisfied()
        {
            DataContext = ViewModel;
        }
    }
}
