using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObservableObjectLibrary;
using System.Collections.ObjectModel;

namespace ObservableObjectTest
{
    public class CollectionToPropertyDependencyTestObject : ObservableObject
    {
        public ObservableCollection<string> items = new ObservableCollection<string>();

        [DependsUpon("items")]
        public int ItemCount
        {
            get { return items.Count; }
        }
    }
}
