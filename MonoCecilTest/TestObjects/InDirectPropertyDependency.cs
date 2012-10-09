using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TestObjects
{
    public class InDirectPropertyDependency : INotifyPropertyChanged
    {
        public int Prop1 { get; set; }
        public int Prop2 { get; set; }

        public int Prop3
        {
            get { return GetProp1() + Prop2; }
        }

        public int Prop4
        {
            get { return Prop2 + Prop3; }
        }

        private int GetProp1()
        {
            return Prop1;
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
