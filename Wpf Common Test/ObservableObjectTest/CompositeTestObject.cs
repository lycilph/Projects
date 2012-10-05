using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObservableObjectLibrary;
using System.Collections.ObjectModel;

namespace ObservableObjectTest
{
    // This tests the following situation
    // ViewModel (here CompositeTestObject) contains a
    // - Settings class (here ChildObject) which contains a
    // -- MRU class (here ChildChildObject) which contains an ObservableCollection

    public class CompositeTestObject : ObservableObject
    {
        public class ChildObject : ObservableObject
        {
            public class ChildChildObject : ObservableObject
            {
                private readonly ObservableCollection<string> _Items = new ObservableCollection<string>();
                public ObservableCollection<string> Items
                {
                    get { return _Items; }
                }
            }

            public readonly ChildChildObject child_child_object = new ChildChildObject();
        }

        private readonly ChildObject child_object = new ChildObject();

        public ObservableCollection<string> Items
        {
            get { return child_object.child_child_object.Items; }
        }

        [DependsUpon("Items")]
        public int Count
        {
            get { return Items.Count; }
        }
    }
}
