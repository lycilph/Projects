using System.ComponentModel;
using System.Runtime.CompilerServices;
using AsyncWebTest.Annotations;

namespace AsyncWebTest
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                if (_IsBusy == value) return;
                _IsBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }

        private string _BusyText;
        public string BusyText
        {
            get { return _BusyText; }
            set
            {
                if (_BusyText == value) return;
                _BusyText = value;
                NotifyPropertyChanged("BusyText");
            }
        }

        private string _Date;
        public string Date
        {
            get { return _Date; }
            set
            {
                if (_Date == value) return;
                _Date = value;
                NotifyPropertyChanged("Date");
            }
        }

        private WeekMenuCollection _WeekMenus;
        public WeekMenuCollection WeekMenus
        {
            get { return _WeekMenus; }
            set
            {
                if (_WeekMenus == value) return;
                _WeekMenus = value;
                NotifyPropertyChanged("WeekMenus");
            }
        }

        private MenuCollection _Menus;
        public MenuCollection Menus
        {
            get { return _Menus; }
            set
            {
                if (_Menus == value) return;
                _Menus = value;
                NotifyPropertyChanged("Menus");
            }
        }

        public ViewModel()
        {
            WeekMenus = new WeekMenuCollection();
            Menus = new MenuCollection();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
