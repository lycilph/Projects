using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifyPropertyWeaver.Interfaces;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace NotifyPropertyWeaverUnitTest
{
    public class A
    {
        public ObservableCollection<int> collection = new ObservableCollection<int>();
    }
}
