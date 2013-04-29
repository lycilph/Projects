using System.ComponentModel.Composition;
using LunchViewer.ViewModels;

namespace LunchViewer.Views
{
    public partial class SettingsAppearance : IPartImportsSatisfiedNotification
    {
        [Import]
        private SettingsAppearanceViewModel ViewModel { get; set; }

        public SettingsAppearance()
        {
            InitializeComponent();
        }

        public void OnImportsSatisfied()
        {
            DataContext = ViewModel;
        }
    }
}
