using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;

namespace NotifyPropertyWeaverUnitTest
{
    //[NotifyPropertyChanged]
    //public class CollectionAutoPropertyDirectDependencyObject : INotifyPropertyChanged
    //{
    //    public ObservableCollection<int> Items { get; set; }

    //    public int Count
    //    {
    //        get { return Items.Count; }
    //    }

    //    public CollectionAutoPropertyDirectDependencyObject()
    //    {
    //        Items = new ObservableCollection<int>();
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected void NotifyPropertyChanged(string property_name)
    //    {
    //        var handler = PropertyChanged;
    //        if (handler != null)
    //            handler(this, new PropertyChangedEventArgs(property_name));
    //    }
    //}

    //[TestClass]
    //public class CollectionAutoPropertyDirectDependencyTest
    //{
    //    [TestMethod]
    //    public void CollectionAutoPropertyDirectDependency()
    //    {
    //        var changed_properties = new List<string>();
    //        var obj = new CollectionAutoPropertyDirectDependencyObject();
    //        obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

    //        obj.Items.Add(42);
    //        Assert.AreEqual(1, changed_properties.Count);
    //        Assert.IsTrue(changed_properties.Contains("Count"));
    //    }
    //}
}
