using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;

namespace NotifyPropertyWeaverUnitTest
{
    [NotifyPropertyChanged]
    public class CollectionAutoPropertyIndirectDependencyObject : INotifyPropertyChanged
    {
        public ObservableCollection<int> Items { get; set; }

        public int Count
        {
            get { return PublicMethod(); }
        }

        private int PrivateMethod() { return Items.Count; }
        protected int ProtectedMethod() { return PrivateMethod(); }
        public int PublicMethod() { return ProtectedMethod(); }

        public CollectionAutoPropertyIndirectDependencyObject()
        {
            Items = new ObservableCollection<int>();
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
    public class CollectionAutoPropertyIndirectDependencyTest
    {
        [TestMethod]
        public void CollectionAutoPropertyIndirectDependency()
        {
            var changed_properties = new List<string>();
            var obj = new CollectionAutoPropertyIndirectDependencyObject();
            obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

            obj.Items.Add(42);
            Assert.AreEqual(1, changed_properties.Count);
            Assert.IsTrue(changed_properties.Contains("Count"));
        }
    }
}
