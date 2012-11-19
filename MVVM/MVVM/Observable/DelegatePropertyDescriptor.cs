using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MVVM.Expressions;

namespace MVVM.Observable
{
    public class DelegatePropertyDescriptor : PropertyDescriptor
    {
        private readonly Type owner_type;
        private readonly Type property_type;
        private readonly string descriptor_name;
        private readonly ViewModelBase view_model_base;

        private PropertyAccessTree property_access_tree;

        public Func<object, object> Getter { get; set; }
        public Action<object, object> Setter { get; set; }

        public DelegatePropertyDescriptor(string descriptor_name, ViewModelBase view_model_base, Type property_type) : base(descriptor_name, null)
        {
            this.owner_type = view_model_base.GetType();
            this.property_type = property_type;
            this.descriptor_name = descriptor_name;
            this.view_model_base = view_model_base;
        }

        #region PropertyDescriptor implementation

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return owner_type; }
        }

        public override object GetValue(object component)
        {
            if (Getter == null)
                throw new NotImplementedException();
            return Getter(component);
        }

        public override bool IsReadOnly
        {
            get { return Setter == null; }
        }

        public override Type PropertyType
        {
            get { return property_type; }
        }

        public override void ResetValue(object component) {}

        public override void SetValue(object component, object value)
        {
            if (Setter == null)
                throw new NotImplementedException();
            Setter(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override string Name
        {
            get { return descriptor_name; }
        }

        #endregion

        #region Dependencies

        public void AddDependencies(PropertyAccessTree tree, Action notification_callback)
        {
            property_access_tree = tree;
            property_access_tree.Subscribe(notification_callback);
        }

        public void Unsubscribe()
        {
            if (property_access_tree != null)
                property_access_tree.Unsubscribe();
        }

        #endregion
    }
}
