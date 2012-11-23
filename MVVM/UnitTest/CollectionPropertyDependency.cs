using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Observable;
using System.Collections.ObjectModel;

namespace UnitTest
{
    [TestClass]
    public class CollectionPropertyDependency
    {
        public class Item : ObservableObject
        {
            private int _Prop;
            public int Prop
            {
                get { return _Prop; }
                set { SetAndRaiseIfChanged(() => Prop, value); }
            }
        }

        public class Model : ObservableObject
        {
            private ObservableCollection<Item> _Items = new ObservableCollection<Item>();
            public ObservableCollection<Item> Items
            {
                get { return _Items; }
                set { SetAndRaiseIfChanged(() => Items, value); }
            }
        }

        public class ViewModel : ViewModelBase
        {
            public ViewModel(Model m)
            {
                Collection("Items", () => m.Items).Each((item, view_model) => view_model.Property("PropSquared", () => item.Prop * item.Prop));
            }
        }

        [TestMethod]
        public void CollectionPropertyDependencyTest()
        {
            List<string> property_notifications = new List<string>();
            Model m = new Model();
            m.Items.Add(new Item() { Prop = 42 });
            m.Items.Add(new Item() { Prop = 23 });
            m.Items.Add(new Item() { Prop = 17 });
            ViewModel vm = new ViewModel(m);

            m.PropertyChanged += (sender, args) => property_notifications.Add("Model:" + args.PropertyName);
            vm.PropertyChanged += (sender, args) => property_notifications.Add("ViewModel:" + args.PropertyName);

            var item = new Item() { Prop = 1 };
            item.PropertyChanged += (sender, args) => property_notifications.Add("Item:" + args.PropertyName);
            m.Items.Add(item);

            Assert.AreEqual(1, property_notifications.Count);
            Assert.IsTrue(property_notifications.Contains("ViewModel:Items"));
            property_notifications.Clear();

            item.Prop = 42;

            Assert.AreEqual(3, property_notifications.Count);
            Assert.IsTrue(property_notifications.Contains("Item:Prop"));
            Assert.IsTrue(property_notifications.Contains("ViewModel:Items")); // Called by Item.Prop changed
            Assert.IsTrue(property_notifications.Contains("ViewModel:Items")); // Called by ItemViewModel.PropSquared
            property_notifications.Clear();

            m.Items.Remove(item);

            Assert.AreEqual(1, property_notifications.Count);
            Assert.IsTrue(property_notifications.Contains("ViewModel:Items"));
        }
    }
}
