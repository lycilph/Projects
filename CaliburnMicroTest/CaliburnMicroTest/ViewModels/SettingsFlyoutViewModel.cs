using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroTest.ViewModels
{
    [Export(typeof(FlyoutBase))]
    public class SettingsFlyoutViewModel : FlyoutBase
    {
        private IEventAggregator event_aggregator;

        [ImportingConstructor]
        public SettingsFlyoutViewModel(IEventAggregator event_aggregator) : base("Settings", Position.Right)
        {
            this.event_aggregator = event_aggregator;
        }

        public void SendMessage()
        {
            event_aggregator.PublishOnCurrentThread(new NavigationMessage(NavigationMessage.NavigationType.First));
            IsOpen = false;
        }
    }
}
