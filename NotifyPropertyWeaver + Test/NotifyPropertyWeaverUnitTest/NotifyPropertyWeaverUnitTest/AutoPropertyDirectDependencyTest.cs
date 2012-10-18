using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;

namespace NotifyPropertyWeaverUnitTest
{
    [NotifyPropertyChanged]
    public class AutoPropertyDirectDependencyObject : INotifyPropertyChanged
    {
        public int Prop1 { get; set; }

        public int Prop2
        {
            get { return Prop1; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property_name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property_name));
        }
    }

    [TestClass]
    public class AutoPropertyDirectDependencyTest
    {
        [TestMethod]
        public void AutoPropertyDirectDependency()
        {
            var changed_properties = new List<string>();
            var obj = new AutoPropertyDirectDependencyObject();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop1 = 42;
            Assert.AreEqual(2, changed_properties.Count);
            Assert.IsTrue(changed_properties.Contains("Prop1"));
            Assert.IsTrue(changed_properties.Contains("Prop2"));
        }
    }
}
