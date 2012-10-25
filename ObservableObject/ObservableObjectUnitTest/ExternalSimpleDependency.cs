using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;

namespace ObservableObjectUnitTest
{
    public class ExternalSimpleDependencyBaseObject : ObservableObject
    {
        protected int _BaseProp1;
        public int BaseProp1
        {
            get { return _BaseProp1; }
            set { SetAndRaiseIfChanged(() => BaseProp1, value); }
        }
    }

    public class ExternalSimpleDependencyObject : ObservableObject
    {
        protected ExternalSimpleDependencyBaseObject _BaseObject;
        public ExternalSimpleDependencyBaseObject BaseObject
        {
            get { return _BaseObject; }
            set { SetAndRaiseIfChanged(() => BaseObject, value); }
        }

        public int Prop1
        {
            get { return BaseObject.BaseProp1; }
        }

        public ExternalSimpleDependencyObject(ExternalSimpleDependencyBaseObject base_object)
        {
            BaseObject = base_object;

            AddDependency(() => BaseObject.BaseProp1, () => Prop1);
        }
    }

    [TestClass]
    public class ExternalSimpleDependency
    {
        [TestMethod]
        public void ExternalSimpleDependencyTest()
        {
            var changed_properties = new List<string>();
            var base_obj = new ExternalSimpleDependencyBaseObject();
            var obj = new ExternalSimpleDependencyObject(base_obj);
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            base_obj.BaseProp1 = 42;

            Assert.AreEqual(1, changed_properties.Count, "1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
            changed_properties.Clear();
            
            base_obj = new ExternalSimpleDependencyBaseObject();
            obj.BaseObject = base_obj;
            base_obj.BaseProp1 = 42;

            Assert.AreEqual(2, changed_properties.Count, "2 property changed events expected");
            Assert.IsTrue(changed_properties.Contains("BaseObject"), "BaseObject property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
        }
    }
}
