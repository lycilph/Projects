using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;

namespace ObservableObjectUnitTest
{
    public class InternalObservableObjectPropertyDependencyBaseObject : ObservableObject
    {
        protected int _BaseProp1;
        public int BaseProp1
        {
            get { return _BaseProp1; }
            set { SetAndRaiseIfChanged(() => BaseProp1, value); }
        }
    }

    public class InternalObservableObjectPropertyDependencyObject : ObservableObject
    {
        protected InternalObservableObjectPropertyDependencyBaseObject _BaseObject;
        public InternalObservableObjectPropertyDependencyBaseObject BaseObject
        {
            get { return _BaseObject; }
            set { SetAndRaiseIfChanged(() => BaseObject, value); }
        }

        public int Prop1
        {
            get { return 42; }
        }

        public InternalObservableObjectPropertyDependencyObject(InternalObservableObjectPropertyDependencyBaseObject base_object)
        {
            BaseObject = base_object;

            AddDependency(() => BaseObject.BaseProp1, () => Prop1);
        }
    }

    [TestClass]
    public class InternalObservableObjectPropertyDependency
    {
        [TestMethod]
        public void InternalObservableObjectPropertyDependencyTest()
        {
            var changed_properties = new List<string>();
            var base_obj = new InternalObservableObjectPropertyDependencyBaseObject();
            var obj = new InternalObservableObjectPropertyDependencyObject(base_obj);
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            base_obj.BaseProp1 = 42;

            Assert.AreEqual(1, changed_properties.Count, "1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
            changed_properties.Clear();

            base_obj = new InternalObservableObjectPropertyDependencyBaseObject();
            obj.BaseObject = base_obj;
            base_obj.BaseProp1 = 23;

            Assert.AreEqual(2, changed_properties.Count, "2 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("BaseObject"), "BaseObject property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
        }
    }
}
