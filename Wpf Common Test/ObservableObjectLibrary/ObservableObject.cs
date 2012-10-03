using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservableObjectLibrary
{
    public class ObservableObject : INotifyPropertyChanged
    {
        private const BindingFlags MemberFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private readonly Dictionary<string, object> values = new Dictionary<string, object>();
        // Source to targets dependencies map
        private readonly Dictionary<string, List<string>> property_dependency_map = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<string>> method_dependency_map = new Dictionary<string, List<string>>();
        // Weak events for dependencies to child objects
        private readonly List<PropertyChangedEventListener> property_changed_listeners = new List<PropertyChangedEventListener>();
        private readonly List<CollectionChangedEventListener> collection_changed_event_listeners = new List<CollectionChangedEventListener>(); 

        public ObservableObject()
        {
            MapDependencies();
        }

        public void UpdateDependencies()
        {
            ClearDependencies();
            MapDependencies();
        }

        private void ClearDependencies()
        {
            property_dependency_map.Clear();
            method_dependency_map.Clear();
            RemoveAllWeakEventListeners();
        }

        #region Dependency mapping

        private void MapDependencies()
        {
            var members = GetType().GetMembers(MemberFlags);
            foreach (var member in members)
            {
                var attributes = member.GetCustomAttributes(typeof (DependsUponAttribute), true)
                                       .Cast<DependsUponAttribute>()
                                       .ToList();

                if (attributes.Any())
                {
                    foreach (var attribute in attributes)
                        MapDependency(member, attribute);
                }
            }
        }

        private void MapDependency(MemberInfo member, DependsUponAttribute attribute)
        {
            // Supported cases
            // - [x] to property dependencies
            // -- Property to property dependency (property_dependency_map)
            // -- INotifyCollectionChanged field to property dependency (collection_changed_event_listeners)
            // -- INotifyPropertyChanged field to property dependency (property_changed_listeners)
            // --- With optional property
            // --- Without optional property
            // - [x] to method dependencies
            // -- Property to method dependency (method_dependency_map)
            // -- INotifyCollectionChanged field to method dependency (collection_changed_event_listeners)
            // -- INotifyPropertyChanged field to method dependency (property_changed_listeners)
            // --- With optional property
            // --- Without optional property

            // Property dependencies
            if (IsProperty(member))
            {
                // Property to property dependency (property_dependency_map)
                if (IsProperty(attribute.Source) && !attribute.HasObject)
                {
                    AddDependency(property_dependency_map, member.Name, attribute.Source);
                    return;
                }

                // INotifyCollectionChanged field to property dependency (collection_changed_event_listeners)
                var collection_source = GetField<INotifyCollectionChanged>(attribute.Source);
                if (collection_source != null && !attribute.HasObject)
                {
                    AddWeakEventListener(collection_source, (sender, args) => NotifyPropertyChanged(member.Name));
                    return;
                }

                // INotifyPropertyChanged field to property dependency (property_changed_listeners)
                var property_source = (attribute.HasObject
                                           ? GetField<INotifyPropertyChanged>(attribute.Object)   // - With optional property
                                           : GetField<INotifyPropertyChanged>(attribute.Source)); // - Without optional property
                if (property_source != null)
                {
                    var handler = (attribute.HasObject
                                       // - With optional property
                                       ? (PropertyChangedEventHandler) ((sender, args) =>
                                                                            {
                                                                                if (args.PropertyName == attribute.Source)
                                                                                    NotifyPropertyChanged(member.Name);
                                                                            })
                                       // - Without optional property
                                       : (PropertyChangedEventHandler) ((sender, args) => NotifyPropertyChanged(member.Name)));

                    AddWeakEventListener(property_source, handler);
                    return;
                }
            }
            // Method dependencies
            else if (IsMethod(member))
            {
                // Property to method dependency (method_dependency_map)
                if (IsProperty(attribute.Source) && !attribute.HasObject)
                {
                    AddDependency(method_dependency_map, member.Name, attribute.Source);
                    return;
                }

                // INotifyCollectionChanged field to method dependency (collection_changed_event_listeners)
                var collection_source = GetField<INotifyCollectionChanged>(attribute.Source);
                if (collection_source != null && !attribute.HasObject)
                {
                    AddWeakEventListener(collection_source, (sender, args) => ExecuteMethod(member.Name));
                    return;
                }

                // INotifyPropertyChanged field to method dependency (property_changed_listeners)
                var property_source = (attribute.HasObject
                                          ? GetField<INotifyPropertyChanged>(attribute.Object)   // - With optional property
                                          : GetField<INotifyPropertyChanged>(attribute.Source)); // - Without optional property
                if (property_source != null)
                {
                    var handler = (attribute.HasObject
                                       // - With optional property
                                       ? (PropertyChangedEventHandler) ((sender, args) =>
                                                                            {
                                                                                if (args.PropertyName == attribute.Source)
                                                                                    ExecuteMethod(member.Name);
                                                                            })
                                       // - Without optional property
                                       : (PropertyChangedEventHandler) ((sender, args) => ExecuteMethod(member.Name)));

                    AddWeakEventListener(property_source, handler);
                    return;
                }
            }

            throw new ArgumentException(string.Format("Dependency {0} is not supported on member {1}", attribute, member.Name));
        }

        private T GetField<T>(string name) where T : class
        {
            var field_info = GetType().GetField(name, MemberFlags);
            if (field_info != null)
            {
                // Does the source implement T
                var field = field_info.GetValue(this) as T;
                if (field != null)
                    return field;
            }
            return null;
        }

        private void AddDependency(IDictionary<string, List<string>> map, string target, string source)
        {
            if (!map.ContainsKey(source))
                map.Add(source, new List<string>());
            map[source].Add(target);
        }

        #endregion

        #region Get and Set

        private static string PropertyName<T>(Expression<Func<T>> expression)
        {
            var member_expression = expression.Body as MemberExpression;

            if (member_expression == null)
                throw new ArgumentException("Expression must be a property expression");

            return member_expression.Member.Name;
        }

        protected T Get<T>(string name)
        {
            return Get<T>(name, default(T));
        }

        protected T Get<T>(Expression<Func<T>> expression)
        {
            return Get<T>(PropertyName(expression));
        }

        protected T Get<T>(string name, T default_value)
        {
            if (values.ContainsKey(name))
                return (T)values[name];

            return default_value;
        }

        protected T Get<T>(Expression<Func<T>> expression, T default_value)
        {
            return Get<T>(PropertyName(expression), default_value);
        }

        protected T Get<T>(string name, Func<T> initial_value)
        {
            if (values.ContainsKey(name))
                return (T)values[name];

            Set(name, initial_value());
            return Get<T>(name);
        }

        protected T Get<T>(Expression<Func<T>> expression, Func<T> initial_value)
        {
            return Get<T>(PropertyName(expression), initial_value);
        }

        protected void Set<T>(string name, T value)
        {
            if (values.ContainsKey(name))
            {
                if (values[name] == null && value == null)
                    return;

                if (values[name] != null && values[name].Equals(value))
                    return;

                values[name] = value;
            }
            else
            {
                values.Add(name, value);
            }

            NotifyPropertyChanged(name);
        }

        protected void Set<T>(Expression<Func<T>> expression, T value)
        {
            Set(PropertyName(expression), value);
        }

        #endregion

        #region Verification and identification

        private bool IsMethod(MemberInfo member)
        {
            return member.MemberType == MemberTypes.Method;
        }

        private bool IsProperty(MemberInfo member)
        {
            return member.MemberType == MemberTypes.Property;
        }

        private bool IsProperty(string name)
        {
            return GetType().GetProperty(name, MemberFlags) != null;
        }

        private void VerifyPropertyDependency(string name)
        {
            if (!IsProperty(name))
                throw new ArgumentException("Property " + name + " does not exist on object " + GetType().Name);
        }

        #endregion

        #region Weak event handling

        protected void RemoveAllWeakEventListeners()
        {
            for (int i = property_changed_listeners.Count - 1; i >= 0; i--)
            {
                var listener = property_changed_listeners[i];
                RemoveWeakEventListener(listener.Source, listener.Handler);
            }
            for (int i = collection_changed_event_listeners.Count - 1; i >= 0; i--)
            {
                var listener = collection_changed_event_listeners[i];
                RemoveWeakEventListener(listener.Source, listener.Handler);
            }
        }

        protected void AddWeakEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (handler == null) throw new ArgumentNullException("handler");

            // Add listener to internal list
            PropertyChangedEventListener listener = new PropertyChangedEventListener(source, handler);
            property_changed_listeners.Add(listener);

            // Add listener to event manager
            PropertyChangedEventManager.AddListener(source, listener, "");
        }

        protected void RemoveWeakEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (handler == null) { throw new ArgumentNullException("handler"); }

            // Find listener in internal list
            PropertyChangedEventListener listener = property_changed_listeners.LastOrDefault(l => l.Source == source && l.Handler == handler);

            if (listener != null)
            {
                // Remove listener from internal list and event manager
                property_changed_listeners.Remove(listener);
                PropertyChangedEventManager.RemoveListener(source, listener, "");
            }
        }

        protected void AddWeakEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (handler == null) { throw new ArgumentNullException("handler"); }

            // Add listener to internal list
            CollectionChangedEventListener listener = new CollectionChangedEventListener(source, handler);
            collection_changed_event_listeners.Add(listener);

            // Add listener to event manager
            CollectionChangedEventManager.AddListener(source, listener);
        }

        protected void RemoveWeakEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (handler == null) { throw new ArgumentNullException("handler"); }

            // Find listener in internal list
            CollectionChangedEventListener listener = collection_changed_event_listeners.LastOrDefault(l => l.Source == source && l.Handler == handler);

            if (listener != null)
            {
                // Remove listener from internal list and event manager
                collection_changed_event_listeners.Remove(listener);
                CollectionChangedEventManager.RemoveListener(source, listener);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string property_name)
        {
            // Raise event for property
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property_name));

            // Raise event for dependent properties
            if (property_dependency_map.ContainsKey(property_name))
                foreach (var property in property_dependency_map[property_name])
                    NotifyPropertyChanged(property);

            // Execute dependent methods
            if (method_dependency_map.ContainsKey(property_name))
                foreach (var method in method_dependency_map[property_name])
                    ExecuteMethod(method);
        }

        private void ExecuteMethod(string method)
        {
            var memberInfo = GetType().GetMethod(method, MemberFlags);
            if (memberInfo == null)
                return;

            memberInfo.Invoke(this, null);
        }

        public void Register(string property_name, Action action)
        {
            VerifyPropertyDependency(property_name);

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == property_name)
                    action();
            };
        }

        #endregion
    }
}
