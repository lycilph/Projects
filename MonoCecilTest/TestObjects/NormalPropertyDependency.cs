using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoCecilRewriter.Interfaces;
using System.ComponentModel;

namespace TestObjects
{
    [NotifyPropertyChanged]
    public class NormalPropertyDependency : INotifyPropertyChanged
    {
        private int field1 = 0;
        public int Prop1
        {
            get { return field1; }
            set { field1 = value; }
        }

        public int Prop2
        {
            get { return Prop1*Prop1; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property_name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
