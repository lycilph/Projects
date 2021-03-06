﻿using ObservableObjectLibrary;
using System.Collections.ObjectModel;

namespace ObservableObjectTest
{
    public class UpdateDependenciesTestObject : ObservableObject
    {
        public ObservableCollection<string> Items { get; set; }
        
        public int Count
        {
            get { return Get(() => Count); }
            set { Set(() => Count, value); }
        }

        [DependsUpon("Items")]
        private void Update()
        {
            Count = Items.Count;
        }

        public UpdateDependenciesTestObject() : base(false)
        {
            Items = new ObservableCollection<string>();
            Update();
            MapDependencies();
        }
    }
}
