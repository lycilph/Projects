using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestObjects;

namespace UnitTests
{
    [TestClass]
    public class InDirectPropertyDependencyTest
    {
        [TestMethod]
        public void InDirectPropertyDependency()
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
    }
}
