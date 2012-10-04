using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservableObjectLibrary
{
    public class ObservableObjectV2 : INotifyPropertyChanged
    {
        private const BindingFlags MemberFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public ObservableObjectV2()
        {
            var members = GetType().GetMembers(MemberFlags);
            foreach (var member in members)
            {
                var attributes = member.GetCustomAttributes(typeof(DependsUponV2Attribute), true)
                                       .Cast<DependsUponV2Attribute>()
                                       .ToList();

                if (attributes.Any())
                {
                    foreach (var attribute in attributes)
                    {
                        ParseAttribute(attribute);
                    }
                }
            }
        }

        private void ParseAttribute(DependsUponV2Attribute attribute)
        {
            var elements = attribute.Source.Split(new char[] {'.'});

            var member = 
        }

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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string property_name)
        {
            // Raise event for property
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property_name));
        }

        public void Register(string property_name, Action action)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == property_name)
                    action();
            };
        }

        #endregion
    }
}
