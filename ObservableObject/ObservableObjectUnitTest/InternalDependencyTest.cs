using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;
using System.Collections.Generic;

namespace ObservableObjectUnitTest
{
  public class InternalDependencyObject : ObservableObject
  {
    private int _Prop1;
    public int Prop1
    {
      get { return _Prop1; }
      set
      {
        if (_Prop1 != value)
        {
          NotifyPropertyChanging("Prop1");
          _Prop1 = value;
          NotifyPropertyChanged("Prop1");
        }
      }
    }

    public int Prop2
    {
      get { return Prop1 * 2; }
    }

    public InternalDependencyObject()
    {
      AddDependency(() => Prop1, () => Prop2);
    }
  }

  [TestClass]
  public class InternalDependencyTest
  {
    [TestMethod]
    public void InternalDependency()
    {
      var changed_properties = new List<string>();
      var obj = new InternalDependencyObject();
      obj.PropertyChanged += (sender, args) => changed_properties.Add(args.PropertyName);

      obj.Prop1 = 42;

      Assert.AreEqual(2, changed_properties.Count, "2 property changed events expected");
      Assert.IsTrue(changed_properties.Contains("Prop1"), "Prop1 property changed event expected");
      Assert.IsTrue(changed_properties.Contains("Prop2"), "Prop2 property changed event expected");
    }
  }
}
