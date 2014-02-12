using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CaliburnMicroTest.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<PageBase>.Collection.OneActive, IHandle<NavigationMessage>, IHandle<BusyMessage>, IShell, IPartImportsSatisfiedNotification
    {
        private static ILog logger = LogManager.GetLog(typeof(ShellViewModel));
        private int overlay_count = 0;

        [Import]
        private IWindowManager WindowManager { get; set; }

        [Import]
        private IEventAggregator EventAggregator { get; set; }

        [ImportMany]
        private IEnumerable<Lazy<PageBase, PageBaseMetadata>> UnsortedPages { get; set; }

        [ImportMany]
        public IEnumerable<FlyoutBase> FlyoutViewModels { get; private set; }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                if (_IsBusy == value) return;
                _IsBusy = value;
                NotifyOfPropertyChange(() => IsBusy);

                if (_IsBusy)
                    ShowOverlay();
                else
                    HideOverlay();
            }
        }

        public bool CanShowMessageBox
        {
            get { return (ActiveItem is ImagesPageViewModel); }
        }

        public bool IsOverlayVisible
        {
            get { return overlay_count > 0; }
        }

        public ShellViewModel()
        {
            DisplayName = "Main window";
        }

        protected override void OnActivationProcessed(PageBase item, bool success)
        {
            base.OnActivationProcessed(item, success);
            NotifyOfPropertyChange(() => CanShowMessageBox);
        }

        public void ShowMessageBox()
        {
            WindowManager.ShowMetroMessageBox("Testing", "Dialog box");
        }

        public void ShowFlyout(string name)
        {
            var flyout = FlyoutViewModels.FirstOrDefault(f => f.Header == name);
            if (flyout != null)
                flyout.Toggle();
        }

        public void OnImportsSatisfied()
        {
            Items.AddRange(UnsortedPages.OrderBy(lazy => lazy.Metadata.Order).Select(lazy => lazy.Value));

            EventAggregator.Subscribe(this);

            logger.Info("Imports satified");
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public void ShowOverlay()
        {
            overlay_count++;
            NotifyOfPropertyChange(() => IsOverlayVisible);
        }

        public void HideOverlay()
        {
            overlay_count--;
            NotifyOfPropertyChange(() => IsOverlayVisible);
        }

        public void Handle(NavigationMessage message)
        {
            switch (message.Type)
            {
                case NavigationMessage.NavigationType.First:
                    ActivateItem(Items.First());
                    break;
                default:
                    break;
            }
        }

        public void Handle(BusyMessage message)
        {
            IsBusy = message.IsBusy;
        }
    }
}
