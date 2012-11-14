using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MVVM.Observable
{
    public class ViewModelBaseTypeDescriptionProvider : TypeDescriptionProvider
    {
        public override ICustomTypeDescriptor GetTypeDescriptor(Type object_type, object instance)
        {
            var vm = (ViewModelBase)instance;
            return new ViewModelBaseTypeDescriptor(vm);
        }
    }
}
