using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableObjectLibrary;
using System;

namespace ObservableObjectUnitTest
{
    public class CleanupDependenciesBaseObject : ObservableObject
    {
        protected int _BaseProp1;
        public int BaseProp1
        {
            get { return _BaseProp1; }
            set { SetAndRaiseIfChanged(() => BaseProp1, value); }
        }
    }

    public class CleanupDependenciesObject : ObservableObject
    {
        protected CleanupDependenciesBaseObject _BaseObject;
        public CleanupDependenciesBaseObject BaseObject
        {
            get { return _BaseObject; }
            set { SetAndRaiseIfChanged(() => BaseObject, value); }
        }

        protected int _Prop1;
        public int Prop1
        {
            get { return _Prop1; }
            set { SetAndRaiseIfChanged(() => Prop1, value); }
        }

        public CleanupDependenciesObject(CleanupDependenciesBaseObject base_object)
        {
            BaseObject = base_object;

            AddDependency(() => BaseObject.BaseProp1, () => Prop1);
        }
    }

    [TestClass]
    public class CleanupDependencies
    {
        [TestMethod]
        public void CleanupDependenciesTest()
        {
            var base_obj = new CleanupDependenciesBaseObject();
            WeakReference obj_reference = null;

            new Action(() =>
                           {
                               var obj = new CleanupDependenciesObject(base_obj);

                               obj_reference = new WeakReference(obj);
                               Assert.IsTrue(obj_reference.IsAlive, "CleanupDependenciesObject must be alive");

                               obj.CleanupDependencies();
                           })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsFalse(obj_reference.IsAlive, "CleanupDependenciesObject must NOT be alive");
        }
    }
}
