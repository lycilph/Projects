using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Threading;

namespace Wpf_ReactiveUI_test
{
    public class Model : ReactiveObject
    {
        private string _Text = string.Empty;
        public string Text
        {
            get { return _Text; }
            set { this.RaiseAndSetIfChanged(x => x.Text, value); }
        }

        private string _SearchText = string.Empty;
        public string SearchText
        {
            get { return _SearchText; }
            set { this.RaiseAndSetIfChanged(x => x.SearchText, value); }
        }

        public ObservableCollection<string> SearchHistory { get; private set; }

        public Model()
        {
            SearchHistory = new ObservableCollection<string>();

            this.ObservableForProperty<Model, string>(x => x.Text)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Select(x => x.Value)
                .Subscribe(x => SearchText = x);

            this.ObservableForProperty<Model, string>(x => x.Text)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Select(x => x.Value)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x => SearchHistory.Add(x));
        }
    }
}
