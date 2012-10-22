using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;
using System.ComponentModel;

namespace NotifyPropertyWeaverUnitTest
{
    //[NotifyPropertyChanged]
    //public class DerivedClassDirectDependencyBaseObject : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected void NotifyPropertyChanged(string property_name)
    //    {
    //        var handler = PropertyChanged;
    //        if (handler != null)
    //            handler(this, new PropertyChangedEventArgs(property_name));
    //    }
    //}

    //public class DerivedClassDirectDependencyObject : DerivedClassDirectDependencyBaseObject
    //{
    //    public int Prop1 { get; set; }

    //    public int Prop2
    //    {
    //        get { return Prop1; }
    //    }
    //}

    //public class DerivedDerivedClassDirectDependencyObject : DerivedClassDirectDependencyObject
    //{
    //    public int Prop11 { get; set; }

    //    public int Prop22
    //    {
    //        get { return Prop11; }
    //    }
    //}

    //[TestClass]
    //public class DerivedClassDirectDependencyTest
    //{
    //    [TestMethod]
    //    public void DerivedClassDirectDependency()
    //    {
    //        var changed_properties = new List<string>();
    //        var obj = new DerivedClassDirectDependencyObject();
    //        obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

    //        obj.Prop1 = 42;
    //        Assert.AreEqual(2, changed_properties.Count);
    //        Assert.IsTrue(changed_properties.Contains("Prop1"));
    //        Assert.IsTrue(changed_properties.Contains("Prop2"));
    //    }

    //    [TestMethod]
    //    public void DerivedDerivedClassDirectDependency()
    //    {
    //        var changed_properties = new List<string>();
    //        var obj = new DerivedDerivedClassDirectDependencyObject();
    //        obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

    //        obj.Prop11 = 42;
    //        Assert.AreEqual(2, changed_properties.Count);
    //        Assert.IsTrue(changed_properties.Contains("Prop11"));
    //        Assert.IsTrue(changed_properties.Contains("Prop22"));
    //        changed_properties.Clear();

    //        obj.Prop1 = 42;
    //        Assert.AreEqual(2, changed_properties.Count);
    //        Assert.IsTrue(changed_properties.Contains("Prop1"));
    //        Assert.IsTrue(changed_properties.Contains("Prop2"));
    //    }
    //}
}
