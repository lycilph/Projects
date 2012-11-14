using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;

namespace ObservableObjectUnitTest
{
    public class InternalObservableObjectDependencyBaseObject : ObservableObject
    {
        protected int _BaseProp1;
        public int BaseProp1
        {
            get { return _BaseProp1; }
            set { SetAndRaiseIfChanged(() => BaseProp1, value); }
        }
    }

    public class InternalObservableObjectDependencyObject : ObservableObject
    {
        protected InternalObservableObjectDependencyBaseObject _BaseObject;
        public InternalObservableObjectDependencyBaseObject BaseObject
        {
            get { return _BaseObject; }
            set { SetAndRaiseIfChanged(() => BaseObject, value); }
        }

        public int Prop1
        {
            get { return 42; }
        }

        public InternalObservableObjectDependencyObject(InternalObservableObjectDependencyBaseObject base_object)
        {
            BaseObject = base_object;

            AddDependency(() => BaseObject, () => Prop1);
        }
    }

    [TestClass]
    public class InternalObservableObjectDependency
    {
        [TestMethod]
        public void InternalObservableObjectDependencyTest()
        {
            var changed_properties = new List<string>();
            var base_obj = new InternalObservableObjectDependencyBaseObject();
            var obj = new InternalObservableObjectDependencyObject(base_obj);
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            base_obj.BaseProp1 = 42;

            Assert.AreEqual(1, changed_properties.Count, "1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
            changed_properties.Clear();

            base_obj = new InternalObservableObjectDependencyBaseObject();
            obj.BaseObject = base_obj;
            base_obj.BaseProp1 = 23;

            Assert.AreEqual(2, changed_properties.Count, "2 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("BaseObject"), "BaseObject property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
        }
    }
}
