using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Observable;

namespace UnitTest
{
    [TestClass]
    public class GarbageCollection
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
        public void GarbageCollectionTest()
        {
            Model m = new Model();
            WeakReference vm_ref = null;

            new Action(() =>
            {
                ViewModel vm = new ViewModel(m);
                vm_ref = new WeakReference(vm);

                vm.Unsubscribe();
            })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsFalse(vm_ref.IsAlive);
        }
    }
}
