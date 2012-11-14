using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace ObservableObjectLibrary
{
    public static class ExpressionHelper
    {
        public static BindingFlags Flags = BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;

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

        public static object FindSourceObject<T>(Expression<Func<T>> source_expression, int stop_level = 0)
        {
            // Unpack lambda expression
            var expr = source_expression.Body;

            // "descend" toward's the root object reference and push on stack
            var member_stack = new Stack<MemberInfo>();
            while (expr is MemberExpression)
            {
                var member_expr = expr as MemberExpression;
                member_stack.Push(member_expr.Member);
                expr = member_expr.Expression;
            }

            // Find root object
            var root_expression = expr as ConstantExpression;
            if (root_expression == null)
                throw new ArgumentException("No root object found");
            var root_object = root_expression.Value;

            // "ascend" back whence we came from and resolve object references along the way
            while (member_stack.Count > stop_level)
            {
                var mi = member_stack.Pop();
                if (mi.MemberType == MemberTypes.Field)
                    root_object = root_object.GetType().GetField(mi.Name, Flags).GetValue(root_object);
                else if (mi.MemberType == MemberTypes.Property)
                    root_object = root_object.GetType().GetProperty(mi.Name, Flags).GetValue(root_object, null);
            }

            return root_object;
        }

        public static bool DependensOn<T>(Expression<Func<T>> source_expression, string property_name)
        {
            // Unpack lambda expression
            var expr = source_expression.Body;

            // "descend" toward's the root object reference and check each expression
            while (expr is MemberExpression)
            {
                var member_expr = expr as MemberExpression;
                if (member_expr.Member.Name == property_name)
                    return true;
                expr = member_expr.Expression;
            }

            return false;
        }
    }
}
