using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;
using NLog;

namespace MVVM.Expressions
{
    public class ExpressionAnalyzer : ExpressionVisitor
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private Stack<Node> current_branch_nodes;
        private PropertyAccessTree tree;

        public static PropertyAccessTree Analyze<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            log.Trace("Analysis starting");

            ExpressionAnalyzer analyzer = new ExpressionAnalyzer()
                                              {
                                                  current_branch_nodes = new Stack<Node>(),
                                                  tree = new PropertyAccessTree()
                                              };

            log.Trace("Building property access tree");
            analyzer.Visit(expression);
            analyzer.tree.DumpToLog();

            log.Trace("Cleaning up tree");
            analyzer.CleanupTree(analyzer.tree.Children);
            analyzer.tree.DumpToLog();

            log.Trace("Filtering tree");
            analyzer.FilterTree(analyzer.tree.Children, DoesTypeImplementINotifyPropertyChangedAndChanging);
            analyzer.tree.DumpToLog();

            log.Trace("Analysis done");

            return analyzer.tree;
        }

        #region Visit members

        public override Expression Visit(Expression node)
        {
            if (tree == null)
                throw new Exception("Please use the Analyze method");

            return base.Visit(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            log.Trace("Visiting lambda expression " + node.ToString());

            //This is done to ensure that the tree has all the parameters and in the same order.
            foreach (var parameter in node.Parameters)
                tree.Children.Add(new ParameterNode(parameter.Type, parameter.Name));

            return base.VisitLambda<T>(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            log.Trace("Visiting member expression " + node.ToString());

            PropertyInfo property = node.Member as PropertyInfo;
            if (property != null)
            {
                current_branch_nodes.Push(new PropertyNode(property));
                return base.VisitMember(node);
            }
            else
                throw new NotImplementedException(string.Format("Not implemented yet ({0})", node.ToString()));
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            log.Trace("Visiting parameter expression " + node.ToString());

            current_branch_nodes.Push(new ParameterNode(node.Type, node.Name));
            CreateCurrentBranch();

            return base.VisitParameter(node);
        } 

        #endregion

        #region Tree creation

        private static bool DoesTypeImplementINotifyPropertyChangedAndChanging(Type type)
        {
            return typeof(INotifyPropertyChanged).IsAssignableFrom(type) &&
                   typeof(INotifyPropertyChanging).IsAssignableFrom(type);
        }

        private void CreateCurrentBranch()
        {
            log.Trace(string.Format("Creating branch ({0} elements)", current_branch_nodes.Count));

            if (current_branch_nodes.Count == 0)
                return;

            Node current_node = current_branch_nodes.Pop();
            tree.Children.Add(current_node);

            while (current_branch_nodes.Count != 0)
            {
                Node next_node = current_branch_nodes.Pop();
                current_node.Children.Add(next_node);
                current_node = next_node;
            }
        }

        private void CleanupTree(IList<Node> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = nodes.Count - 1; j > i; j--)
                {
                    if (nodes[i].IsDuplicate(nodes[j]))
                    {
                        nodes[i].Children.AddRange(nodes[j].Children);
                        nodes.RemoveAt(j);
                    }
                }
                CleanupTree(nodes[i].Children);
            }
        }

        private void FilterTree(IList<Node> nodes, Predicate<Type> type_filter)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                var property_node = nodes[i] as PropertyNode;
                if (property_node != null)
                {
                    if (property_node.Children.Count > 0 && !type_filter(property_node.Property.PropertyType))
                        property_node.Children.Clear();
                }
                FilterTree(nodes[i].Children, type_filter);
            }
        }

        #endregion
    }
}
