using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableObjectLibrary
{
    public class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged, INotifyDependenciesUpdated
    {
        public ObservableObject()
        {
            UpdateDependenciesNeeded = false;
            UpdatingProperty = string.Empty;

            PropertyChanging += PropertyChangingHandler;
            PropertyChanged += PropertyChangedHandler;
        }

        public void Cleanup()
        {
            // Remove all dependencies
            foreach (var dependency in dependencies)
                dependency.Remove();
            dependencies.Clear();

            // Remove all DependenciesUpdated events
            var properties = GetType().GetProperties(ExpressionHelper.Flags);
            foreach (var property in properties)
            {
                if (property.PropertyType.IsSubclassOf(typeof(ObservableObject)))
                {
                    var property_value = property.GetValue(this, null);
                    if (property_value != null)
                    {
                        var property_object = property_value as ObservableObject;
                        if (property_object != null)
                            property_object.DependenciesUpdated -= UpdateDependencies;
                    }
                }
            }

            // Remove object level property handlers
            PropertyChanging -= PropertyChangingHandler;
            PropertyChanged -= PropertyChangedHandler;
        }

        #region Dependencies tracking

        private readonly List<DependencyBase> dependencies = new List<DependencyBase>();
        private bool UpdateDependenciesNeeded;
        private string UpdatingProperty;

        private void PropertyChangingHandler(Object sender, PropertyChangingEventArgs args)
        {
            var property_info = GetType().GetProperty(args.PropertyName, ExpressionHelper.Flags);

            var is_notifycollectionchanged = property_info.PropertyType.GetInterfaces().Contains(typeof(INotifyCollectionChanged));
            var is_observableobject = property_info.PropertyType.IsSubclassOf(typeof(ObservableObject));

            // Remove DependenciesUpdated event if needed
            if (is_observableobject)
            {
                var property_value = property_info.GetValue(this, null);
                if (property_value != null)
                {
                    var property_object = property_value as ObservableObject;
                    if (property_object != null)
                        property_object.DependenciesUpdated -= UpdateDependencies;
                }
            }

            if (is_notifycollectionchanged || is_observableobject)
            {
                UpdateDependenciesNeeded = true;
                UpdatingProperty = property_info.Name;
            }
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (UpdateDependenciesNeeded && args.PropertyName == UpdatingProperty)
            {
                // Add DependenciesUpdated event if needed
                var property_info = GetType().GetProperty(args.PropertyName, ExpressionHelper.Flags);
                var is_observableobject = property_info.PropertyType.IsSubclassOf(typeof(ObservableObject));
                if (is_observableobject)
                {
                    var property_value = property_info.GetValue(this, null);
                    if (property_value != null)
                    {
                        var property_object = property_value as ObservableObject;
                        if (property_object != null)
                            property_object.DependenciesUpdated += UpdateDependencies;
                    }
                }

                UpdateDependencies(UpdatingProperty);

                // Notify others that a dependency has changed
                NotifyDependenciesUpdated(UpdatingProperty);

                UpdateDependenciesNeeded = false;
                UpdatingProperty = string.Empty;
            }
        }

        private void UpdateDependencies(string property)
        {
            // Find all dependencies for that property (and remove them)
            var dependencies_to_update = dependencies.Where(d => d.DependensOn(property)).ToList();
            dependencies.RemoveAll(d => d.DependensOn(property));

            // Update dependencies for that property
            foreach (var dependency in dependencies_to_update)
                dependency.Update(this);
        }

        private void UpdateDependencies(object sender, DependenciesUpdatedEventArgs args)
        {
            UpdateDependencies(args.SourceName);
        }

        #endregion

        #region Add dependency

        public void AddDependency<T1, T2>(Expression<Func<T1>> source, Expression<Func<T2>> target)
        {
            var is_notifycollectionchanged = typeof(T1).GetInterfaces().Contains(typeof(INotifyCollectionChanged));
            var is_observableobject = typeof(T1).IsSubclassOf(typeof(ObservableObject));

            if (is_notifycollectionchanged)
            {
                // Parse input
                INotifyCollectionChanged source_object = ExpressionHelper.FindSourceObject(source) as INotifyCollectionChanged;
                string target_property_name = ExpressionHelper.FindPropertyName(target);

                // Check that the source object is an INotifyCollectionChanged
                if (source_object == null)
                    throw new ArgumentException("Source object not of type " + typeof(INotifyCollectionChanged).ToString());

                // Check that the source property is actually is a property
                ExpressionHelper.CheckPropertyName(source);

                // Create dependency event handler
                NotifyCollectionChangedEventHandler handler = (sender, args) => NotifyPropertyChanged(target_property_name);

                // Add dependency to the object itself
                source_object.CollectionChanged += handler;

                // Cache the dependency
                var dependency = DependencyHelper.CreateCollectionDependency(source, target, source_object, handler);
                dependencies.Add(dependency);
            }
            else if (is_observableobject)
            {
                // Parse input
                ObservableObject source_object = ExpressionHelper.FindSourceObject(source) as ObservableObject;
                string target_property_name = ExpressionHelper.FindPropertyName(target);

                // Check that the source object is an ObservableObject
                if (source_object == null)
                    throw new ArgumentException("Source object not of type " + typeof(ObservableObject).ToString());

                // Check that the source property is actually is a property
                ExpressionHelper.CheckPropertyName(source);

                // Create dependency event handler
                PropertyChangedEventHandler handler = (sender, args) => NotifyPropertyChanged(target_property_name);

                // Add dependency to the object itself
                source_object.PropertyChanged += handler;

                // Cache the dependency
                var dependency = DependencyHelper.CreatePropertyDependency(source, target, source_object, handler);
                dependencies.Add(dependency);
            }
            else
            {
                // Parse input
                ObservableObject source_object = ExpressionHelper.FindSourceObject(source, 1) as ObservableObject; // stop_level = 1, as the source property will be the last of the stack
                string source_property_name = ExpressionHelper.FindPropertyName(source);
                string target_property_name = ExpressionHelper.FindPropertyName(target);

                // Check that the source object is an ObservableObject
                if (source_object == null)
                    throw new ArgumentException("Source object not of type " + typeof(ObservableObject).ToString());

                // Create dependency event handler
                PropertyChangedEventHandler handler = (sender, args) =>
                {
                    if (args.PropertyName == source_property_name)
                        NotifyPropertyChanged(target_property_name);
                };

                // Add dependency to the object itself
                source_object.PropertyChanged += handler;

                // Cache the dependency
                var dependency = DependencyHelper.CreatePropertyDependency(source, target, source_object, handler);
                dependencies.Add(dependency);
            }
        }

        #endregion

        #region Interface implementation

        public event PropertyChangingEventHandler PropertyChanging;
        public void NotifyPropertyChanging(string property_name)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (handler != null)
                handler(this, new PropertyChangingEventArgs(property_name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string property_name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property_name));
        }

        public event DependenciesUpdatedEventHandler DependenciesUpdated;
        public void NotifyDependenciesUpdated(string property_name)
        {
            DependenciesUpdatedEventHandler handler = DependenciesUpdated;
            if (handler != null)
                handler(this, new DependenciesUpdatedEventArgs(property_name));
        }

        #endregion

        #region Set and raise if changed

        // Modelled from ReactiveUI
        protected void SetAndRaiseIfChanged<T>(Expression<Func<T>> property, T value)
        {
            string property_name = ExpressionHelper.FindPropertyName(property);
            string backing_field_name = string.Format("_{0}", property_name);

            var backing_field = GetType().GetField(backing_field_name, ExpressionHelper.Flags);
            if (backing_field == null)
                throw new ArgumentException(string.Format("Unabled to find backing field for property {0}", property_name));

            var backing_field_value = backing_field.GetValue(this);

            if (EqualityComparer<T>.Default.Equals((T)backing_field_value, value))
                return;

            NotifyPropertyChanging(property_name);
            backing_field.SetValue(this, value);
            NotifyPropertyChanged(property_name);
        }

        #endregion
    }
}
