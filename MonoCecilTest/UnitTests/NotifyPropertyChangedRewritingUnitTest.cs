using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestObjects;

namespace UnitTests
{
    [TestClass]
    public class NotifyPropertyChangedRewritingUnitTest
    {
        [TestMethod]
        public void InDirectPropertyDependencyTest()
        {
            var changed_properties = new List<string>();
            var obj = new InDirectPropertyDependency();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop1 = 42;
            Assert.IsTrue(changed_properties.Contains("Prop1"));
            Assert.IsTrue(changed_properties.Contains("Prop3"));
            changed_properties.Clear();

            obj.Prop2 = 42;
            Assert.IsTrue(changed_properties.Contains("Prop2"));
            Assert.IsTrue(changed_properties.Contains("Prop3"));
        }

        [TestMethod]
        public void DependsOnCollectionTest()
        {
            var changed_properties = new List<string>();
            var obj = new DependsOnCollection();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Items.Add("Item 1");
            Assert.IsTrue(changed_properties.Contains("Count"));
        }

        [TestMethod]
        public void WrapFieldTest()
        {
            var changed_properties = new List<string>();
            var obj = new WrapField();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Field = 1;
            Assert.IsTrue(changed_properties.Contains("Field"));
            changed_properties.Clear();

            obj.Property = 1;
            Assert.IsTrue(changed_properties.Contains("Property"));
        }
    }
}
