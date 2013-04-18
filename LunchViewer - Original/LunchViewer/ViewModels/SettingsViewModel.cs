using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using LunchViewer.Annotations;
using LunchViewer.Infrastructure;

namespace LunchViewer.ViewModels
{
    [Export(typeof(SettingsViewModel))]
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private bool _IsOpen;
        public bool IsOpen
        {
            get { return _IsOpen; }
            set
            {
                if (value.Equals(_IsOpen)) return;
                _IsOpen = value;
                NotifyPropertyChanged();
            }
        }

        private string _CurrentCulture;
        public string CurrentCulture
        {
            get { return _CurrentCulture; }
            set
            {
                if (value == _CurrentCulture) return;
                _CurrentCulture = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<string> Cultures { get; private set; }

        public SettingsViewModel()
        {
            //Cultures = LocalizationService.GetAvailableCultures();
            //CurrentCulture = Cultures.First();
        }

        private void Apply()
        {
            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = CultureInfo.CreateSpecificCulture(CurrentCulture);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
