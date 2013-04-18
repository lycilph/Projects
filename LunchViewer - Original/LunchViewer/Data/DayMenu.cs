using System.ComponentModel;
using System.Runtime.CompilerServices;
using LunchViewer.Annotations;

namespace LunchViewer.Data
{
    public class DayMenu : INotifyPropertyChanged
    {
        public WeekMenu Parent { get; set; }

        public string Day { get; set; }
        public string Text { get; set; }
        
        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                if (value.Equals(_IsExpanded)) return;
                _IsExpanded = value;
                NotifyPropertyChanged();
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value.Equals(_IsSelected)) return;
                _IsSelected = value;
                NotifyPropertyChanged();
            }
        }

        public DayMenu()
        {
            IsExpanded = false;
            IsSelected = false;
        }

        public void Select()
        {
            IsSelected = true;
            Parent.IsExpanded = true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
