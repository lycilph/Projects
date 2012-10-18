using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;

namespace NotifyPropertyWeaverUnitTest
{
    [NotifyPropertyChanged]
    public class ChildPropertyDependencyChildObject : INotifyPropertyChanged
    {
        public int PropChild { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property_name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property_name));
        }
    }

    [TestClass]
    public class ChildPropertyDependencyTest
    {
        [TestMethod]
        public void WrapField()
        {
            
        }
    }
}
