using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;
using System.ComponentModel;

namespace NotifyPropertyWeaverUnitTest
{
    [NotifyPropertyChanged]
    public class NormalPropertyDirectDependencyObject : INotifyPropertyChanged
    {
        private int _Prop1;
        public int Prop1
        {
            get { return _Prop1; }
            set { _Prop1 = value; }
        }

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
    public class NormalPropertyDirectDependencyTest
    {
        [TestMethod]
        public void NormalPropertyDirectDependency()
        {
            var changed_properties = new List<string>();
            var obj = new NormalPropertyDirectDependencyObject();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Prop1 = 42;
            Assert.AreEqual(2, changed_properties.Count);
            Assert.IsTrue(changed_properties.Contains("Prop1"));
            Assert.IsTrue(changed_properties.Contains("Prop2"));
        }
    }
}
