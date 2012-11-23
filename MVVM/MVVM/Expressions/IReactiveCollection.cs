using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM.Observable;

namespace MVVM.Expressions
{
    public interface IReactiveCollection<T>
    {
        IReactiveCollection<T> Each(Action<T, ItemViewModel> transformation);
    }
}
