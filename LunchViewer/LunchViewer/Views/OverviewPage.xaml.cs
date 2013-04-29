using System.ComponentModel.Composition;
using LunchViewer.ViewModels;

namespace LunchViewer.Views
{
    public partial class OverviewPage : IPartImportsSatisfiedNotification
    {
        [Import]
        private OverviewViewModel ViewModel { get; set; }

        public OverviewPage()
        {
            InitializeComponent();
        }

        public void OnImportsSatisfied()
        {
            DataContext = ViewModel;
        }
    }
}
