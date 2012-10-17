using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestObjects;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class MonoCecilRewriterTest
    {
        [TestMethod]
        public void NoPropertyChangedMethodDefinedTest()
        {
            var changed_properties = new List<string>();
            var obj = new NoNotifyPropertyChangedMethodClass();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop = 42;
            Assert.IsTrue(changed_properties.Contains("Prop"));
            Assert.AreEqual(1, changed_properties.Count);
        }

        [TestMethod]
        public void InDirectPropertyDependencyTest()
        {
            var changed_properties = new List<string>();
            var obj = new InDirectPropertyDependency();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop1 = 42;
            Assert.IsTrue(changed_properties.Contains("Prop1"));
            Assert.IsTrue(changed_properties.Contains("Prop3"));
            Assert.IsTrue(changed_properties.Contains("Prop4"));
            Assert.AreEqual(3, changed_properties.Count);
            changed_properties.Clear();

            obj.Prop2 = 42;
            Assert.IsTrue(changed_properties.Contains("Prop2"));
            Assert.IsTrue(changed_properties.Contains("Prop3"));
            Assert.IsTrue(changed_properties.Contains("Prop4"));
            Assert.AreEqual(3, changed_properties.Count);
        }

        [TestMethod]
        public void DependsOnAutoCollectionPropertyTest()
        {
            var changed_properties = new List<string>();
            var obj = new DependsOnAutoCollectionProperty();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Items.Add("Item 1");
            Assert.IsTrue(changed_properties.Contains("Count"));
            Assert.AreEqual(1, changed_properties.Count);
        }

        [TestMethod]
        public void DependsOnNormalCollectionPropertyTest()
        {
            var changed_properties = new List<string>();
            var obj = new DependsOnNormalCollectionProperty();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Items.Add("Item 1");
            Assert.IsTrue(changed_properties.Contains("Count"));
            Assert.AreEqual(1, changed_properties.Count);
        }

        [TestMethod]
        public void NormalPropertyDependencyTest()
        {
            var changed_properties = new List<string>();
            var obj = new NormalPropertyDependency();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop1 = 42;
            Assert.IsTrue(changed_properties.Contains("Prop1"));
            Assert.IsTrue(changed_properties.Contains("Prop2"));
            Assert.AreEqual(2, changed_properties.Count);
        }

        [TestMethod]
        public void ExternalPropertyDependencyTest()
        {
            var changed_properties = new List<string>();
            var obj = new ExternalPropertyDependency();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop1 = 42;
            Assert.IsTrue(changed_properties.Contains("Prop1"));
            Assert.AreEqual(1, changed_properties.Count);
        }
    }
}
