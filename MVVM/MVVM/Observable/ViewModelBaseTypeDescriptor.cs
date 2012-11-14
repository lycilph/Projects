using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MVVM.Observable
{
    public class ViewModelBaseTypeDescriptor : CustomTypeDescriptor
    {
        private readonly ViewModelBase view_model_base;

        public ViewModelBaseTypeDescriptor(ViewModelBase vm)
        {
            view_model_base = vm;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            var properties = base.GetProperties().Cast<PropertyDescriptor>();
            properties = properties.Union(view_model_base.GetProperties());
            return new PropertyDescriptorCollection(properties.ToArray());
        }
    }
}
