using System.ComponentModel;
using System.Collections.Generic;

namespace Wp_filter_controls_test
{
    public class FilterBase : ObservableObject
    {
        #region Properties
        private ICollectionView view = null;

        public string Name = string.Empty;

        private List<FilterOperator> _Operators = new List<FilterOperator>();
        public List<FilterOperator> Operators
        {
            get { return _Operators; }
            set { _Operators = value; }
        }

        private FilterOperator _CurrentOperator;
        public FilterOperator CurrentOperator
        {
            get { return _CurrentOperator; }
            set
            {
                if (_CurrentOperator == value)
                    return;
                _CurrentOperator = value;
                NotifyPropertyChanged("CurrentOperator");
                Refresh();
            }
        }
        

        private string _Input1 = string.Empty;
        public string Input1
        {
            get { return _Input1; }
            set
            {
                if (_Input1 == value)
                    return;
                _Input1 = value;
                NotifyPropertyChanged("Input1");
                Refresh();
            }
        }

        private string _Input2 = string.Empty;
        public string Input2
        {
            get { return _Input2; }
            set
            {
                if (_Input2 == value)
                    return;
                _Input2 = value;
                NotifyPropertyChanged("Input2");
                Refresh();
            }
        }

        private bool _IsValid = true;
        public bool IsValid
        {
            get { return _IsValid; }
            set
            {
                if (_IsValid == value)
                    return;
                _IsValid = value;
                NotifyPropertyChanged("IsValid");
            }
        }
        #endregion

        public void AttachTo(ICollectionView cv)
        {
            view = cv;
            view.Filter = Filter;
            Refresh();
        }

        public virtual bool Filter(object o)
        {
            return true;
        }

        public void Refresh()
        {
            if (view != null)
                view.Refresh();
        }
    }
}
