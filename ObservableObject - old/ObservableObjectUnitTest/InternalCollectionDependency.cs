using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;

namespace ObservableObjectUnitTest
{
    public class InternalCollectionDependencyObject : ObservableObject
    {
        protected ObservableCollection<int> _Prop1;
        public ObservableCollection<int> Prop1
        {
            get { return _Prop1; }
            set { SetAndRaiseIfChanged(() => Prop1, value); }
        }

        public int Prop2
        {
            get { return Prop1.Count; }
        }

        public InternalCollectionDependencyObject()
        {
            Prop1 = new ObservableCollection<int>();

            AddDependency(() => Prop1, () => Prop2);
        }
    }

    [TestClass]
    public class InternalCollectionDependency
    {
        [TestMethod]
        public void InternalCollectionDependencyTest()
        {
            var changed_properties = new List<string>();
            var obj = new InternalCollectionDependencyObject();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop1.Add(42);

            Assert.AreEqual(1, changed_properties.Count, "1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop2"), "Prop2 property changed event expected");
            changed_properties.Clear();
            
            obj.Prop1 = new ObservableCollection<int>();
            obj.Prop1.Add(23);

            Assert.AreEqual(2, changed_properties.Count, "2 property changed events expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop2"), "Prop2 property changed event expected");
        }
    }
}
