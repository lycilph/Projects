using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Observable;
using System.ComponentModel;

namespace UnitTest
{
    [TestClass]
    public class ModelPropertyDependency
    {
        public class Model : ObservableObject
        {
            private int _Prop1;
            public int Prop1
            {
                get { return _Prop1; }
                set { SetAndRaiseIfChanged(() => Prop1, value); }
            }

            private int _Prop2;
            public int Prop2
            {
                get { return _Prop2; }
                set { SetAndRaiseIfChanged(() => Prop2, value); }
            }
        }

        public class ViewModel : ViewModelBase
        {
            public ViewModel(Model m)
            {
                Property("Total", () => m.Prop1 + m.Prop2);
            }
        }

        [TestMethod]
        public void ModelPropertyDependencyTest()
        {
            List<string> property_notifications = new List<string>();
            Model m = new Model() { Prop1 = 1, Prop2 = 2 };
            ViewModel vm = new ViewModel(m);

            m.PropertyChanged += (sender, args) => property_notifications.Add("Model:" + args.PropertyName);
            vm.PropertyChanged += (sender, args) => property_notifications.Add("ViewModel:" + args.PropertyName);

            m.Prop1 = 42;

            Assert.AreEqual(2, property_notifications.Count);
            Assert.IsTrue(property_notifications.Contains("Model:Prop1"));
            Assert.IsTrue(property_notifications.Contains("ViewModel:Total"));

            var property = TypeDescriptor.GetProperties(vm)["Total"];
            var value = property.GetValue(vm);

            Assert.AreEqual(44, value);
        }
    }
}
