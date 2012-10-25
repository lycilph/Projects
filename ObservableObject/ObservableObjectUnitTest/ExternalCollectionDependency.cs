using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;
using System.Collections.ObjectModel;

namespace ObservableObjectUnitTest
{
    public class ExternalCollectionDependencyBaseObject : ObservableObject
    {
        protected ObservableCollection<int> _BaseProp1;
        public ObservableCollection<int> BaseProp1
        {
            get { return _BaseProp1; }
            set { SetAndRaiseIfChanged(() => BaseProp1, value); }
        }

        public ExternalCollectionDependencyBaseObject()
        {
            BaseProp1 = new ObservableCollection<int>();
        }
    }

    public class ExternalCollectionDependencyObject : ObservableObject
    {
        protected ExternalCollectionDependencyBaseObject _BaseObject;
        public ExternalCollectionDependencyBaseObject BaseObject
        {
            get { return _BaseObject; }
            set { SetAndRaiseIfChanged(() => BaseObject, value); }
        }

        public int Prop1
        {
            get { return BaseObject.BaseProp1.Count; }
        }

        public ExternalCollectionDependencyObject(ExternalCollectionDependencyBaseObject base_object)
        {
            BaseObject = base_object;

            AddDependency(() => BaseObject.BaseProp1, () => Prop1);
        }
    }

    [TestClass]
    public class ExternalCollectionDependency
    {
        [TestMethod]
        public void ExternalCollectionDependencyTest()
        {
            var changed_properties = new List<string>();
            var base_obj = new ExternalCollectionDependencyBaseObject();
            var obj = new ExternalCollectionDependencyObject(base_obj);
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            base_obj.BaseProp1.Add(42);

            Assert.AreEqual(1, changed_properties.Count, "1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
            changed_properties.Clear();

            base_obj = new ExternalCollectionDependencyBaseObject();
            obj.BaseObject = base_obj;
            base_obj.BaseProp1.Add(42);

            Assert.AreEqual(2, changed_properties.Count, "2 property changed events expected");
            Assert.IsTrue(changed_properties.Contains("BaseObject"), "BaseObject property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
            changed_properties.Clear();

            base_obj.BaseProp1 = new ObservableCollection<int>();
            base_obj.BaseProp1.Add(42);

            Assert.AreEqual(1, changed_properties.Count, "1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
        }
    }
}
