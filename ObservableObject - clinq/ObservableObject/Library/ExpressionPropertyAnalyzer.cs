using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservableObject.Library
{
    public static class ExpressionPropertyAnalyzer
    {
        #region Methods

        public static PropertyAccessTree Analyze<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return Analyze(expression, DoesTypeImplementINotifyPropertyChanged);
        }

        public static PropertyAccessTree Analyze<T, TResult>(Expression<Func<T, TResult>> expression, Predicate<Type> type_filter)
        {
            if (!type_filter(typeof(T)))
                return null;

            PropertyAccessTree tree = AnalyzeLambda(expression, type_filter);
            return tree;
        }

        public static PropertyAccessTree Analyze<T0, T1, TResult>(Expression<Func<T0, T1, TResult>> expression)
        {
            PropertyAccessTree tree = AnalyzeLambda(expression, DoesTypeImplementINotifyPropertyChanged);
            return tree;
        }

        private static PropertyAccessTree AnalyzeLambda(LambdaExpression expression, Predicate<Type> type_filter)
        {
            PropertyAccessTree tree = new PropertyAccessTree();
            //This is done to ensure that the tree has all the parameters and in the same order.
            for (int i = 0; i < expression.Parameters.Count; i++)
            {
                ParameterExpression parameterExpression = expression.Parameters[0];
                tree.Children.Add(new ParameterNode(parameterExpression.Type, parameterExpression.Name));
            }
            BuildUnoptimizedTree(tree, expression.Body, type_filter);

            RemoveRedundantNodesFromTree(tree.Children);
            ApplyTypeFilter(tree.Children, type_filter);

            return tree;
        }

        private static void ApplyTypeFilter(List<PropertyAccessTreeNode> children, Predicate<Type> type_filter)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                var propertyAccessNode = children[i] as PropertyAccessNode;
                if (propertyAccessNode != null)
                {
                    if (propertyAccessNode.Children.Count > 0 && !type_filter(propertyAccessNode.Property.PropertyType))
                    {
                        propertyAccessNode.Children.Clear();
                    }
                }
                ApplyTypeFilter(children[i].Children, type_filter);
            }
        }

        private static void RemoveRedundantNodesFromTree(IList<PropertyAccessTreeNode> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = nodes.Count - 1; j > i; j--)
                {
                    if (nodes[i].IsRedundantVersion(nodes[j]))
                    {
                        nodes[i].Children.AddRange(nodes[j].Children);
                        nodes.RemoveAt(j);
                    }
                }
                RemoveRedundantNodesFromTree(nodes[i].Children);
            }
        }

        private static void BuildUnoptimizedTree(PropertyAccessTree tree, Expression expression, Predicate<Type> type_filter)
        {
            var currentNodeBranch = new Stack<PropertyAccessTreeNode>();
            BuildBranches(expression, tree, currentNodeBranch, type_filter);
        }

        private static bool DoesTypeImplementINotifyPropertyChanged(Type type)
        {
            return typeof(INotifyPropertyChanged).IsAssignableFrom(type);
        }

        private static void BuildBranches(Expression expression, PropertyAccessTree tree, Stack<PropertyAccessTreeNode> current_node_branch, Predicate<Type> type_filter)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;

            if (binaryExpression != null)
            {
                BuildBranches(binaryExpression.Left, tree, current_node_branch, type_filter);
                BuildBranches(binaryExpression.Right, tree, current_node_branch, type_filter);
                return;
            }

            UnaryExpression unaryExpression = expression as UnaryExpression;

            if (unaryExpression != null)
            {
                BuildBranches(unaryExpression.Operand, tree, current_node_branch, type_filter);
                return;
            }

            MethodCallExpression methodCallExpression = expression as MethodCallExpression;

            if (methodCallExpression != null)
            {
                foreach (Expression argument in methodCallExpression.Arguments)
                {
                    BuildBranches(argument, tree, current_node_branch, type_filter);
                }
                return;
            }

            ConditionalExpression conditionalExpression = expression as ConditionalExpression;

            if (conditionalExpression != null)
            {
                BuildBranches(conditionalExpression.Test, tree, current_node_branch, type_filter);
                BuildBranches(conditionalExpression.IfTrue, tree, current_node_branch, type_filter);
                BuildBranches(conditionalExpression.IfFalse, tree, current_node_branch, type_filter);
                return;
            }

            InvocationExpression invocationExpression = expression as InvocationExpression;

            if (invocationExpression != null)
            {
                foreach (Expression argument in invocationExpression.Arguments)
                {
                    BuildBranches(argument, tree, current_node_branch, type_filter);
                }
                BuildBranches(invocationExpression.Expression, tree, current_node_branch, type_filter);
                return;
            }

            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    MemberExpression memberExpression = (MemberExpression)expression;

                    PropertyInfo property = memberExpression.Member as PropertyInfo;
                    FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
                    if (property != null)
                    {
                        PropertyAccessNode node = new PropertyAccessNode(property);
                        current_node_branch.Push(node);

                        BuildBranches(memberExpression.Expression, tree, current_node_branch, type_filter);
                    }
                    else if (fieldInfo != null)
                    {
                        if (type_filter(fieldInfo.FieldType))
                        {
                            ConstantExpression constantExpression = (ConstantExpression)memberExpression.Expression;
                            if (constantExpression.Value != null)
                            {
                                object value = fieldInfo.GetValue(constantExpression.Value);
                                ConstantNode constantNode = new ConstantNode((INotifyPropertyChanged)value);
                                current_node_branch.Push(constantNode);
                                AddBranch(tree, current_node_branch);
                            }
                        }
                        else
                        {
                            current_node_branch.Clear();
                        }
                    }
                    else
                    {
                        BuildBranches(memberExpression.Expression, tree, current_node_branch, type_filter);
                    }

                    break;

                case ExpressionType.Parameter:
                    ParameterExpression parameterExpression = (ParameterExpression)expression;
                    ParameterNode parameterNode = new ParameterNode(expression.Type, parameterExpression.Name);
                    current_node_branch.Push(parameterNode);
                    AddBranch(tree, current_node_branch);
                    break;

                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = (ConstantExpression)expression;
                        if (type_filter(constantExpression.Type) &&
                            constantExpression.Value != null)
                        {
                            ConstantNode constantNode = new ConstantNode((INotifyPropertyChanged)constantExpression.Value);
                            current_node_branch.Push(constantNode);
                            AddBranch(tree, current_node_branch);
                        }
                        else
                        {
                            current_node_branch.Clear();
                        }
                    }
                    break;
                case ExpressionType.New:
                    {
                        NewExpression newExpression = (NewExpression)expression;
                        foreach (Expression argument in newExpression.Arguments)
                        {
                            BuildBranches(argument, tree, current_node_branch, type_filter);
                        }
                    }
                    break;
                case ExpressionType.MemberInit:
                    {
                        MemberInitExpression memberInitExpression = (MemberInitExpression)expression;
                        BuildBranches(memberInitExpression.NewExpression, tree, current_node_branch, type_filter);
                        foreach (var memberBinding in memberInitExpression.Bindings)
                        {
                            MemberAssignment assignment = memberBinding as MemberAssignment;
                            if (assignment != null)
                            {
                                BuildBranches(assignment.Expression, tree, current_node_branch, type_filter);
                            }
                        }
                    }
                    break;
                default:
                    throw new InvalidProgramException(string.Format("CLINQ does not support expressions of type: {0}", expression.NodeType));
            }
        }

        private static void AddBranch(PropertyAccessTree tree, Stack<PropertyAccessTreeNode> currentNodeBranch)
        {
            if (currentNodeBranch.Count == 0)
                return;

            PropertyAccessTreeNode currentNode = currentNodeBranch.Pop();
            tree.Children.Add(currentNode);

            while (currentNodeBranch.Count != 0)
            {
                PropertyAccessTreeNode nextNode = currentNodeBranch.Pop();
                currentNode.Children.Add(nextNode);
                currentNode = nextNode;
            }
        }

        #endregion
    }
}
