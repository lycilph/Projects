using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM.Observable;
using System.Collections.ObjectModel;

namespace MVVM.Expressions
{
    public class ReactiveCollection<T> : IReactiveCollection<T>
    {
        private ObservableCollection<T> original_collection;
        private readonly ObservableCollection<ViewModelBase> wrapped_collection = new ObservableCollection<ViewModelBase>();
        // Original to wrapped dictionary
        // Transformation actions

        public ObservableCollection<ViewModelBase> WrappedCollection
        {
            get { return wrapped_collection; }
        }

        public IReactiveCollection<T> Each(Action<T, ViewModelBase> transformation)
        {
            throw new NotImplementedException();
        }
    }
}
