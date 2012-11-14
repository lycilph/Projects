using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using NLog;
using MVVM.Expressions;
using System.Linq.Expressions;

namespace MVVM.Observable
{
    [TypeDescriptionProvider(typeof(ViewModelBaseTypeDescriptionProvider))]
    public abstract class ViewModelBase : ObservableObject
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        #region Runtime properties and dependencies

        private readonly List<PropertyDescriptor> properties = new List<PropertyDescriptor>();

        public void AddProperty(PropertyDescriptor property_descriptor)
        {
            properties.Add(property_descriptor);
        }

        public IEnumerable<PropertyDescriptor> GetProperties()
        {
            return properties.AsReadOnly();
        }

        public void Unsubscribe()
        {
            foreach (var property in properties)
            {
                var delegate_property = property as DelegatePropertyDescriptor;
                if (delegate_property != null)
                    delegate_property.Unsubscribe();
            }
        }
        
        #endregion

        #region Property mapping

        public void Property<T>(string property_name, Expression<Func<T>> getter)
        {
            log.Trace(string.Format("Adding property {0} of type {1}", property_name, typeof(T)));

            var property = new DelegatePropertyDescriptor(property_name, this, typeof(T));
            var compiled_getter = getter.Compile();
            property.Getter = x => compiled_getter();

            // Analyse getter for dependencies
            var tree = ExpressionAnalyzer.Analyze(getter);
            property.AddDependencies(tree, () => NotifyPropertyChanged(property_name));

            AddProperty(property);
        }

        public void AllProperties(object source)
        {
            log.Trace(string.Format("Adding all properties from {0}", source));
    
            var source_properties = TypeDescriptor.GetProperties(source).OfType<PropertyDescriptor>();
            foreach (var source_property in source_properties)
            {
                log.Trace(string.Format("Adding property {0} of type {1}", source_property.Name, source_property.PropertyType));

                var property = new DelegatePropertyDescriptor(source_property.Name, this, source_property.PropertyType);

                var temp_source_property = source_property; // This is needed to keep a local copy for the expressions below
                property.Getter = x => temp_source_property.GetValue(source);
                if (!temp_source_property.IsReadOnly)
                    property.Setter = (x, v) => temp_source_property.SetValue(source, v);

                // Create property access tree directly
                var tree = PropertyAccessTreeFactory.Create(source, source_property.Name);
                property.AddDependencies(tree, () => NotifyPropertyChanged(temp_source_property.Name));
                
                AddProperty(property);
            }
        }

        public void Dependency<T>(Expression<Func<T>> native_property, Expression<Func<T>> dependencies)
        {
            string property_name = FindPropertyName(native_property);
            var property_info = GetType().GetProperty(property_name);

            var property = new DelegatePropertyDescriptor(property_name, this, typeof(T));
            property.Getter = x => property_info.GetValue(this, null);
            if (property_info.CanWrite)
                property.Setter = (x, v) => property_info.SetValue(this, v, null);

            // Analyse getter for dependencies
            var tree = ExpressionAnalyzer.Analyze(dependencies);
            property.AddDependencies(tree, () => NotifyPropertyChanged(property_name));

            AddProperty(property);
        }

        #endregion
    }
}
