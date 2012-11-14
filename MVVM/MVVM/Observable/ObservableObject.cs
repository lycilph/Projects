using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace MVVM.Observable
{
    public abstract class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
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

        protected static BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

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
