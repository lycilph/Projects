using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;

namespace NotifyPropertyWeaverUnitTest
{
    //[NotifyPropertyChanged]
    //public class ChildPropertyDependencyChildObject : INotifyPropertyChanged
    //{
    //    public int PropChild { get; set; }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected void NotifyPropertyChanged(string property_name)
    //    {
    //        var handler = PropertyChanged;
    //        if (handler != null)
    //            handler(this, new PropertyChangedEventArgs(property_name));
    //    }
    //}

    //[NotifyPropertyChanged]
    //public class ChildPropertyDependencyParentObject : INotifyPropertyChanged
    //{
    //    public ChildPropertyDependencyChildObject Child { get; set; }

    //    public int PropParent
    //    {
    //        get { return Child.PropChild; }
    //    }

    //    public ChildPropertyDependencyParentObject()
    //    {
    //        Child = new ChildPropertyDependencyChildObject();
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
    //public class ChildPropertyDependencyTest
    //{
    //    [TestMethod]
    //    public void ChildPropertyDependency()
    //    {
    //        var changed_properties = new List<string>();
    //        var obj = new ChildPropertyDependencyParentObject();
    //        obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

    //        obj.Child.PropChild = 42;
    //        Assert.AreEqual(1, changed_properties.Count);
    //        Assert.IsTrue(changed_properties.Contains("PropParent"));
    //    }
    //}
}
