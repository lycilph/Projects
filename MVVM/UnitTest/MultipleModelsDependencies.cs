using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using MVVM.Observable;

namespace UnitTest
{
    [TestClass]
    public class MultipleModelsDependencies
    {
        public class ModelA : ObservableObject
        {
            private int _PropA;
            public int PropA
            {
                get { return _PropA; }
                set { SetAndRaiseIfChanged(() => PropA, value); }
            }
        }

        public class ModelB : ObservableObject
        {
            private int _PropB;
            public int PropB
            {
                get { return _PropB; }
                set { SetAndRaiseIfChanged(() => PropB, value); }
            }
        }

        public class ViewModel : ViewModelBase
        {
            public ViewModel(ModelA ma, ModelB mb)
            {
                Property("Total", () => ma.PropA + mb.PropB);
            }
        }

        [TestMethod]
        public void MultipleModelsDependenciesTest()
        {
            List<string> property_notifications = new List<string>();
            ModelA ma = new ModelA() { PropA = 1 };
            ModelB mb = new ModelB() { PropB = 2 };
            ViewModel vm = new ViewModel(ma, mb);

            int current_total;

            ma.PropertyChanged += (sender, args) => property_notifications.Add("ModelA:" + args.PropertyName);
            mb.PropertyChanged += (sender, args) => property_notifications.Add("ModelB:" + args.PropertyName);
            vm.PropertyChanged += (sender, args) => property_notifications.Add("ViewModel:" + args.PropertyName);

            ma.PropA = 2;

            Assert.AreEqual(2, property_notifications.Count);
            Assert.IsTrue(property_notifications.Contains("ModelA:PropA"));
            Assert.IsTrue(property_notifications.Contains("ViewModel:Total"));
            property_notifications.Clear();

            mb.PropB = 40;

            Assert.AreEqual(2, property_notifications.Count);
            Assert.IsTrue(property_notifications.Contains("ModelB:PropB"));
            Assert.IsTrue(property_notifications.Contains("ViewModel:Total"));

            var total_property = TypeDescriptor.GetProperties(vm)["Total"];
            var total = total_property.GetValue(vm);
            Assert.AreEqual(42, total);
        }
    }
}
