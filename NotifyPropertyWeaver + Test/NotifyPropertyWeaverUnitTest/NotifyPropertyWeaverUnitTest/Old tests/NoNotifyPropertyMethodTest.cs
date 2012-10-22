using System.ComponentModel;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;

namespace NotifyPropertyWeaverUnitTest
{
    //[NotifyPropertyChanged]
    //public class NoNotifyPropertyMethodObject : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;
    //}

    //[TestClass]
    //public class NoNotifyPropertyMethodTest
    //{
    //    [TestMethod]
    //    public void NoNotifyPropertyMethod()
    //    {
    //        var obj = new NoNotifyPropertyMethodObject();
    //        MemberInfo[] members = obj.GetType().GetMember("NotifyPropertyChanged", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    //        Assert.AreEqual(1, members.Length);
    //    }
    //}
}
