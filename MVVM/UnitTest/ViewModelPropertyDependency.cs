using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Observable;

namespace UnitTest
{
    [TestClass]
    public class ViewModelPropertyDependency
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
            private Model model;

            public int Total
            {
                get { return model.Prop1 + model.Prop2; }
            }

            public ViewModel(Model m)
            {
                this.model = m;

                Dependency(() => Total, () => model.Prop1 + model.Prop2);
            }
        }

        [TestMethod]
        public void ViewModelPropertyDependencyTest()
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

            Assert.AreEqual(44, vm.Total);
        }
    }
}
