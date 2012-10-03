using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Module_Interface;
using System.Windows;

namespace Wp_filter_controls_test
{
    public class FilterControlViewModel : ObservableObject
    {
        private ICollectionView view;

        public ICommand AddFilterCommand { get; private set; }
        public ICommand RemoveFilterCommand { get; private set; }

        public TextFilter SimpleFilter { get; set; }

        private AggregateFilter advanced_filter;
        public ObservableCollection<FilterViewModel> AdvancedFilterViewModels { get; set; }

        private bool _ShowAdvancedView = false;
        public bool ShowAdvancedView
        {
            get { return _ShowAdvancedView; }
            set
            {
                if (_ShowAdvancedView == value)
                    return;
                _ShowAdvancedView = value;
                NotifyPropertyChanged("ShowAdvancedView");
                UpdateFilter();
            }
        }

        public FilterControlViewModel(ICollectionView cv)
        {
            view = cv;

            AddFilterCommand = new RelayCommand(_ => AddAdvancedFilter());
            RemoveFilterCommand = new RelayCommand(RemoveAdvancedFilter, CanRemoveAdvancedFilter);
            
            SimpleFilter = new TextFilter();

            advanced_filter = new AggregateFilter();
            AdvancedFilterViewModels = new ObservableCollection<FilterViewModel>();

            UpdateFilter();
        }

        private bool CanRemoveAdvancedFilter(object o)
        {
            return AdvancedFilterViewModels.Count > 1;
        }
        private void RemoveAdvancedFilter(object o)
        {
            var filter_view_model = o as FilterViewModel;
            if (filter_view_model == null)
                return;

            AdvancedFilterViewModels.Remove(filter_view_model);
            advanced_filter.Remove(filter_view_model.Filter);
        }

        private void AddAdvancedFilter()
        {
            AddAdvancedFilter(new TextFilter());
        }
        private void AddAdvancedFilter(FilterBase filter)
        {
            var view_model = new FilterViewModel(filter, this);
            AdvancedFilterViewModels.Add(view_model);
            advanced_filter.Add(view_model.Filter);
        }

        public void UpdateAdvancedFilter(FilterViewModel filter_view_model)
        {
            // Remove old filter
            advanced_filter.Remove(filter_view_model.Filter, false /*Don't refresh*/);

            // Create and add new filter
            filter_view_model.Filter = FilterFactory.Create(filter_view_model.SelectedFilter);
            advanced_filter.Add(filter_view_model.Filter); // Refresh happens here
        }

        private void UpdateFilter()
        {
            if (ShowAdvancedView)
            {
                advanced_filter.AttachTo(view);
                advanced_filter.Clear();

                if (AdvancedFilterViewModels.Count > 0)
                {
                    foreach (var fvm in AdvancedFilterViewModels)
                        advanced_filter.Add(fvm.Filter);
                }
                else
                {
                    var filter = new TextFilter();
                    if (!string.IsNullOrWhiteSpace(SimpleFilter.Input1))
                        filter.Input1 = SimpleFilter.Input1;
                    AddAdvancedFilter(filter);
                }
            }
            else
            {
                SimpleFilter.AttachTo(view);
            }
        }
    }
}
