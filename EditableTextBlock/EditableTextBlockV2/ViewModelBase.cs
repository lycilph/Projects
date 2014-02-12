using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditableTextBlockV2
{
    public class ViewModelBase<TModel> : ObservableObject, IViewModel<TModel> where TModel : ObservableObject
    {
        public TModel Model { get; private set; }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected == value) return;
                _IsSelected = value;
                NotifyPropertyChanged();
            }
        }

        public ViewModelBase(TModel model)
        {
            Model = model;
            Model.PropertyChanged += ForwardPropertyChanged;
        }

        private void ForwardPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }
    }
}
