using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Observable;

namespace UnitTest
{
    [TestClass]
    public class PropertyForwarding
    {
        public class Model : ObservableObject
        {
            private int _Prop;
            public int Prop
            {
                get { return _Prop; }
                set { SetAndRaiseIfChanged(() => Prop, value); }
            }
        }

        public class ViewModel : ViewModelBase
        {
            public ViewModel(Model m)
            {
                AllProperties(m);
            }
        }

        [TestMethod]
        public void PropertyForwardingTest()
        {
            List<string> property_notifications = new List<string>();
            Model m = new Model();
            ViewModel vm = new ViewModel(m);

            m.PropertyChanged += (sender, args) => property_notifications.Add("Model:" + args.PropertyName);
            vm.PropertyChanged += (sender, args) => property_notifications.Add("ViewModel:" + args.PropertyName);

            m.Prop = 42;

            Assert.AreEqual(2, property_notifications.Count);
            Assert.IsTrue(property_notifications.Contains("Model:Prop"));
            Assert.IsTrue(property_notifications.Contains("ViewModel:Prop"));
        }
    }
}
