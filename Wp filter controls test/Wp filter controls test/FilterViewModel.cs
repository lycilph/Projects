using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wp_filter_controls_test
{
    public class FilterViewModel : ObservableObject
    {
        private FilterControlViewModel filter_control_view_model = null;

        private FilterBase _Filter;
        public FilterBase Filter
        {
            get { return _Filter; }
            set
            {
                if (_Filter == value)
                    return;
                _Filter = value;
                NotifyPropertyChanged("Filter");
            }
        }

        public List<string> AvailableFilters { get; private set; }

        private string _SelectedFilter;
        public string SelectedFilter
        {
            get { return _SelectedFilter; }
            set
            {
                if (_SelectedFilter == value)
                    return;
                _SelectedFilter = value;
                NotifyPropertyChanged("SelectedFilter");
                filter_control_view_model.UpdateAdvancedFilter(this);
            }
        }

        public FilterViewModel(FilterBase f, FilterControlViewModel fcvm)
        {
            Filter = f;
            filter_control_view_model = fcvm;

            AvailableFilters = FilterFactory.GetAvailableFilters();
            _SelectedFilter = Filter.Name; // Don't use the property as that will create a new filter and use that
        }
    }
}
