using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Observable;
using System.Collections.ObjectModel;

namespace UnitTest
{
    [TestClass]
    public class CollectionPropertyDependency
    {
        public class Item : ObservableObject
        {
            private int _Prop;
            public int Prop
            {
                get { return _Prop; }
                set { SetAndRaiseIfChanged(() => Prop, value); }
            }
        }

        public class Model : ObservableObject
        {
            private ObservableCollection<Item> _Items = new ObservableCollection<Item>();
            public ObservableCollection<Item> Items
            {
                get { return _Items; }
                set { SetAndRaiseIfChanged(() => Items, value); }
            }
        }

        public class ViewModel : ViewModelBase
        {
            public ViewModel(Model m)
            {
                Collection("Items", () => m.Items);
            }
        }

        [TestMethod]
        public void CollectionPropertyDependencyTest()
        {
            List<string> property_notifications = new List<string>();
            Model m = new Model();
            ViewModel vm = new ViewModel(m);

            Assert.Fail();
        }
    }
}
