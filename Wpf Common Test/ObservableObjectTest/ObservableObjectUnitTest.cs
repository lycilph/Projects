using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObservableObjectTest
{
    [TestClass]
    public class ObservableObjectUnitTest
    {
        [TestMethod]
        public void PropertyToPropertyDependency()
        {
            var obj = new PropertyToPropertyDependencyTestObject(1);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(1, obj.Prop2);

            var n = 0;
            obj.Register("Prop2", () => n++);
            obj.Prop1 = 42;

            Assert.AreEqual(1, n);
            Assert.AreEqual(42, obj.Prop1);
            Assert.AreEqual(42, obj.Prop2);
        }

        [TestMethod]
        public void PropertyToMethodDependency()
        {
            var obj = new PropertyToMethodDependencyTestObject(1, 2);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(2, obj.Prop2);

            obj.Prop1 = 42;

            Assert.AreEqual(42, obj.Prop1);
            Assert.AreEqual(42, obj.Prop2);
        }

        [TestMethod]
        public void CollectionToPropertyDependency()
        {
            var obj = new CollectionToPropertyDependencyTestObject();
            Assert.AreEqual(0, obj.ItemCount);

            var n = 0;
            obj.Register("ItemCount", () => n++);
            obj.items.Add("Item");

            Assert.AreEqual(1, n);
            Assert.AreEqual(1, obj.ItemCount);
        }

        [TestMethod]
        public void CollectionToMethodDependency()
        {
            var obj = new CollectionToMethodDependencyTestObject();
            Assert.AreEqual(0, obj.ItemCount);

            obj.items.Add("Item");

            Assert.AreEqual(1, obj.ItemCount);
        }

        [TestMethod]
        public void ObjectWithPropertyToPropertyDependencySameProperty()
        {
            var obj = new ObjectWithPropertyToPropertyDependencyTestObject(1, 2);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(1, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);

            var n = 0;
            obj.Register("Prop1", () => n++);
            obj.obj.Prop1 = 42;

            Assert.AreEqual(1, n);
            Assert.AreEqual(42, obj.Prop1);
            Assert.AreEqual(42, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);
        }

        [TestMethod]
        public void ObjectWithPropertyToPropertyDependencyOtherProperty()
        {
            var obj = new ObjectWithPropertyToPropertyDependencyTestObject(1, 2);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(1, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);

            var n = 0;
            obj.Register("Prop1", () => n++);
            obj.obj.Prop2 = 42;

            Assert.AreEqual(0, n);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(1, obj.obj.Prop1);
            Assert.AreEqual(42, obj.obj.Prop2);
        }

        [TestMethod]
        public void ObjectToPropertyDependency()
        {
            var obj = new ObjectToPropertyDependencyTestObject(1, 2);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(1, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);

            var n = 0;
            obj.Register("Prop1", () => n++);
            obj.obj.Prop1 = 23;
            obj.obj.Prop2 = 42;

            Assert.AreEqual(2, n);
            Assert.AreEqual(23, obj.Prop1);
            Assert.AreEqual(23, obj.obj.Prop1);
            Assert.AreEqual(42, obj.obj.Prop2);
        }

        [TestMethod]
        public void ObjectWithPropertyToMethodDependencySameProperty()
        {
            var obj = new ObjectWithPropertyToMethodDependencyTestObject(1, 2);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(1, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);

            obj.Prop1 = 0;
            obj.obj.Prop1 = 42;

            Assert.AreEqual(42, obj.Prop1);
            Assert.AreEqual(42, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);
        }

        [TestMethod]
        public void ObjectWithPropertyToMethodDependencyOtherProperty()
        {
            var obj = new ObjectWithPropertyToMethodDependencyTestObject(1, 2);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(1, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);

            obj.Prop1 = 0;
            obj.obj.Prop2 = 42;

            Assert.AreEqual(0, obj.Prop1);
            Assert.AreEqual(1, obj.obj.Prop1);
            Assert.AreEqual(42, obj.obj.Prop2);
        }

        [TestMethod]
        public void ObjectToMethodDependency()
        {
            var obj = new ObjectToMethodDependencyTestObject(1, 2);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(1, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);

            obj.Prop1 = 0;
            obj.obj.Prop1 = 42;

            Assert.AreEqual(42, obj.Prop1);
            Assert.AreEqual(42, obj.obj.Prop1);
            Assert.AreEqual(2, obj.obj.Prop2);

            obj.Prop1 = 0;
            obj.obj.Prop2 = 23;

            Assert.AreEqual(42, obj.Prop1);
            Assert.AreEqual(42, obj.obj.Prop1);
            Assert.AreEqual(23, obj.obj.Prop2);
        }

        [TestMethod]
        public void ChainedPropertyDependency()
        {
            var obj = new ChainedPropertyDependencyTestObject(1, 2, 3);
            Assert.AreEqual(1, obj.Prop1);
            Assert.AreEqual(2, obj.Prop2);
            Assert.AreEqual(3, obj.Prop3);

            int prop1_call_count = 0;
            int prop2_call_count = 0;
            int prop3_call_count = 0;
            obj.Register("Prop1", () => prop1_call_count++);
            obj.Register("Prop2", () => prop2_call_count++);
            obj.Register("Prop3", () => prop3_call_count++);
            obj.Prop1 = 42;
            obj.Prop2 = 23;
            obj.Prop3 = 17;

            Assert.AreEqual(1, prop1_call_count);
            Assert.AreEqual(2, prop2_call_count);
            Assert.AreEqual(3, prop3_call_count);
            Assert.AreEqual(42, obj.Prop1);
            Assert.AreEqual(23, obj.Prop2);
            Assert.AreEqual(17, obj.Prop3);
        }

        [TestMethod]
        public void DrilldownTest()
        {
            var obj = new CompositeTestObject();

            int count = 0;
            obj.Register("Count", () => count = obj.Items.Count);

            obj.Items.Add("Item 1");
            obj.Items.Add("Item 2");
            obj.Items.Add("Item 3");

            Assert.AreEqual(3, obj.Items.Count);
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void UpdateDependenciesTest()
        {
            var obj = new UpdateDependenciesTestObject();
            Assert.AreEqual(obj.Items.Count, obj.Count);

            int count = 0;
            obj.Register("Count", () => count++);

            obj.Items.Add("Item 1");
            obj.Items.Add("Item 2");
            obj.Items.Add("Item 3");

            Assert.AreEqual(obj.Items.Count, obj.Count);
            Assert.AreEqual(3, count);

        }
    }
}
