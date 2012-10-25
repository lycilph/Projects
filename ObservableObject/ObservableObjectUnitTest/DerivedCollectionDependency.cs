using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;

namespace ObservableObjectUnitTest
{
    public class DerivedCollectionDependencyBaseObject : ObservableObject
    {
        protected ObservableCollection<int> _BaseProp1;
        public ObservableCollection<int> BaseProp1
        {
            get { return _BaseProp1; }
            set { SetAndRaiseIfChanged(() => BaseProp1, value); }
        }

        public DerivedCollectionDependencyBaseObject()
        {
            BaseProp1 = new ObservableCollection<int>();
        }
    }

    public class DerivedCollectionDependencyObject : DerivedCollectionDependencyBaseObject
    {
        public int Prop1
        {
            get { return BaseProp1.Count; }
        }

        public DerivedCollectionDependencyObject()
        {
            AddDependency(() => BaseProp1, () => Prop1);
        }
    }

    [TestClass]
    public class DerivedCollectionDependency
    {
        [TestMethod]
        public void DerivedCollectionDependencyTest()
        {
            var changed_properties = new List<string>();
            var obj = new DerivedCollectionDependencyObject();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.BaseProp1.Add(42);

            Assert.AreEqual(1, changed_properties.Count, "1 property changed events expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
        }
    }
}
