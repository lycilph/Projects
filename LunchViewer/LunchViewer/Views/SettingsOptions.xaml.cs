using System.ComponentModel.Composition;
using LunchViewer.ViewModels;

namespace LunchViewer.Views
{
    public partial class SettingsOptions : IPartImportsSatisfiedNotification
    {
        [Import]
        private SettingsOptionsViewModel ViewModel { get; set; }

        public SettingsOptions()
        {
            InitializeComponent();
        }

        public void OnImportsSatisfied()
        {
            DataContext = ViewModel;
        }
    }
}
