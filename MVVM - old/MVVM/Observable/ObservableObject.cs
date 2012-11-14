using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using MVVM.Expressions;

namespace MVVM.Observable
{
    public abstract class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static Dictionary<Type, IDependency> dependencies { get; set; }

        static ObservableObject()
        {
            dependencies = new Dictionary<Type, IDependency>();
        }

        protected ObservableObject()
        {
            Type type = this.GetType();

            SubscribeToChangesBaseFirst(type);
        }

        private void SubscribeToChangesBaseFirst(Type type)
        {
            if (type == typeof(ObservableObject))
                return;

            SubscribeToChangesBaseFirst(type.BaseType);

            IDependency dependency;
            if (dependencies.TryGetValue(type, out dependency))
                dependency.SubscribeToChanges(this);
        }

        public void UnsubscribeFromChanges()
        {
            UnsubscribeFromChangesDerivedFirst(this.GetType());
        }

        private void UnsubscribeFromChangesDerivedFirst(Type type)
        {
            if (type == typeof(ObservableObject))
                return;

            IDependency dependency;
            if (dependencies.TryGetValue(type, out dependency))
                dependency.UnsubscribeFromChanges(this);

            UnsubscribeFromChangesDerivedFirst(type.BaseType);
        }

        protected static Dependency<T> Register<T>() where T : class, INotifyPropertyChanged, INotifyPropertyChanging
        {
            Type type = typeof(T);

            if (dependencies.ContainsKey(type))
                throw new InvalidOperationException("Type has already been registered for: " + typeof(T));

            var dependency = new Dependency<T>();
            dependencies[type] = dependency;

            return dependency;
        }

        #region Graph debug output

        public IEnumerable<PropertyAccessTree> GraphDebug_GetPropertyAccessTrees()
        {
            IDependency dependency;
            if (dependencies.TryGetValue(this.GetType(), out dependency))
                return dependency.GraphDebug_GetPropertyAccessTrees();

            throw new Exception("Could not find any property access trees");
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

        #endregion

        #region Set and raise if changed

        protected static BindingFlags Flags = BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;

        // Modelled from ReactiveUI
        protected void SetAndRaiseIfChanged<T>(Expression<Func<T>> property, T value)
        {
            string property_name = FindPropertyName(property);
            string backing_field_name = string.Format("_{0}", property_name);

            var backing_field = GetType().GetField(backing_field_name, Flags);
            if (backing_field == null)
                throw new ArgumentException(string.Format("Unabled to find backing field for property {0}", property_name));

            var backing_field_value = backing_field.GetValue(this);

            if (EqualityComparer<T>.Default.Equals((T)backing_field_value, value))
                return;

            NotifyPropertyChanging(property_name);
            backing_field.SetValue(this, value);
            NotifyPropertyChanged(property_name);
        }

        protected static string FindPropertyName<T>(Expression<Func<T>> expression)
        {
            var member_expression = expression.Body as MemberExpression;
            if (member_expression == null)
                throw new ArgumentException(string.Format("Expression ({0}) must be a MemberExpression", expression));

            if (member_expression.Member.MemberType != MemberTypes.Property)
                throw new ArgumentException(string.Format("Expression ({0}) must be a property", expression));

            return member_expression.Member.Name;
        }

        #endregion
    }
}
