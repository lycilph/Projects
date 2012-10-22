using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;

namespace NotifyPropertyWeaverUnitTest
{
    //[NotifyPropertyChanged]
    //public class CollectionNormalPropertyDirectDependencyObject : INotifyPropertyChanged
    //{
    //    private ObservableCollection<int> _Items = new ObservableCollection<int>(); 
    //    public ObservableCollection<int> Items
    //    {
    //        get { return _Items; }
    //        set { _Items = value; }
    //    }

    //    public int Count
    //    {
    //        get { return Items.Count; }
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
    //public class CollectionNormalPropertyDirectDependencyTest
    //{
    //    [TestMethod]
    //    public void CollectionNormalPropertyDirectDependency()
    //    {
    //        var changed_properties = new List<string>();
    //        var obj = new CollectionNormalPropertyDirectDependencyObject();
    //        obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

    //        obj.Items.Add(42);
    //        Assert.AreEqual(1, changed_properties.Count);
    //        Assert.IsTrue(changed_properties.Contains("Count"));
    //    }
    //}
}
