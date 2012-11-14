using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;

namespace ObservableObjectUnitTest
{
    public class DerivedSimpleDependencyBaseObject : ObservableObject
    {
        protected int _BaseProp1;
        public int BaseProp1
        {
            get { return _BaseProp1; }
            set { SetAndRaiseIfChanged(() => BaseProp1, value); }
        }
    }

    public class DerivedSimpleDependencyObject : DerivedSimpleDependencyBaseObject
    {
        public int Prop1
        {
            get { return BaseProp1*2; }
        }

        public DerivedSimpleDependencyObject()
        {
            AddDependency(() => BaseProp1, () => Prop1);
        }
    }

    [TestClass]
    public class DerivedSimpleDependency
    {
        [TestMethod]
        public void DerivedSimpleDependencyTest()
        {
            var changed_properties = new List<string>();
            var obj = new DerivedSimpleDependencyObject();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.BaseProp1 = 42;

            Assert.AreEqual(2, changed_properties.Count, "2 property changed events expected");
            Assert.IsTrue(changed_properties.Contains("BaseProp1"), "BaseProp1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
        }
    }
}
