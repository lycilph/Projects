using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoCecilRewriter.Interfaces;
using System.ComponentModel;

namespace TestObjects
{
    public static class ExternalClass
    {
        public static int ExternalProp { get; set; }
    }

    [NotifyPropertyChanged]
    public class ExternalPropertyDependency : INotifyPropertyChanged
    {
        private int Field1 = 42;
        public int Prop1
        {
            get { return Field1 + ExternalClass.ExternalProp; }
            set { Field1 = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
