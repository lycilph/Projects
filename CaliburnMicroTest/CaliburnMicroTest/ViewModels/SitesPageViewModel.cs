using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CaliburnMicroTest.ViewModels
{
    [Export(typeof(PageBase))]
    [ExportMetadata("Order", 3)]
    public class SitesPageViewModel : PageBase
    {
        private IEventAggregator event_aggregator;

        [ImportingConstructor]
        public SitesPageViewModel(IEventAggregator event_aggregator) : base("Sites")
        {
            this.event_aggregator = event_aggregator;
        }

        public void ShowBusyIndicator()
        {
            event_aggregator.PublishOnCurrentThread(new BusyMessage(true));

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            if (timer == null) return;

            timer.Tick -= timer_Tick;

            event_aggregator.PublishOnCurrentThread(new BusyMessage(false));
        }
    }
}
