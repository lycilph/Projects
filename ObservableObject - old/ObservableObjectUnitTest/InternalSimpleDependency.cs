using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;

namespace ObservableObjectUnitTest
{
    public class InternalSimpleDependencyObject : ObservableObject
    {
        protected int _Prop1;
        public int Prop1
        {
            get { return _Prop1; }
            set { SetAndRaiseIfChanged(() => Prop1, value); }
        }

        public int Prop2
        {
            get { return Prop1 * 2; }
        }

        public InternalSimpleDependencyObject()
        {
            AddDependency(() => Prop1, () => Prop2);
        }
    }

    [TestClass]
    public class InternalSimpleDependency
    {
        [TestMethod]
        public void InternalSimpleDependencyTest()
        {
            var changed_properties = new List<string>();
            var obj = new InternalSimpleDependencyObject();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop1 = 42;

            Assert.AreEqual(2, changed_properties.Count, "2 property changed events expected");
            Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
            Assert.IsTrue(changed_properties.Contains("Prop2"), "Prop2 property changed event expected");
        }
    }
}
