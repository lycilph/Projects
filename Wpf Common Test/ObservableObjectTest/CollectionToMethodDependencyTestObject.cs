using System.Collections.ObjectModel;
using ObservableObjectLibrary;

namespace ObservableObjectTest
{
    public class CollectionToMethodDependencyTestObject : ObservableObject
    {
        public ObservableCollection<string> items = new ObservableCollection<string>();

        public int ItemCount
        {
            get { return Get(() => ItemCount); }
            set { Set(() => ItemCount, value); }
        }

        [DependsUpon("items")]
        private void Update()
        {
            ItemCount = items.Count;
        }
    }
}
