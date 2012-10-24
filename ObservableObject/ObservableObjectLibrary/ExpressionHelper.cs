using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservableObjectLibrary
{
    public static class ExpressionHelper
    {
        public static string FindPropertyName<T>(Expression<Func<T>> expression)
        {
            var member_expression = expression.Body as MemberExpression;
            if (member_expression == null)
                throw new ArgumentException(string.Format("Expression ({0}) must be a MemberExpression", expression));

            if (member_expression.Member.MemberType != MemberTypes.Property)
                throw new ArgumentException(string.Format("Expression ({0}) must be a property", expression));

            return member_expression.Member.Name;
        }

        public static void CheckPropertyName<T>(Expression<Func<T>> expression)
        {
            if (string.IsNullOrEmpty(FindPropertyName(expression)))
                throw new ArgumentException(string.Format("Expression ({0}) must be a property", expression));
        }
    }
}
