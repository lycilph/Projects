using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using MVVM.Expressions;
using System.Collections.Generic;
using MVVM.Subscribers;
using NLog;

namespace MVVM.Observable
{
    public class DependencyMethod<T> where T : class, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly Action<T> callback;
        private List<PropertyAccessTreeSubscriber<DependencyMethod<T>>> property_change_subscribers;

        internal DependencyMethod(Action<T> callback)
        {
            this.callback = callback;
        }

        public DependencyMethod<T> OnChanged<TResult>(Expression<Func<T, TResult>> property_accessor)
        {
            // Build property access tree from expression
            PropertyAccessTree property_access_tree = ExpressionAnalyzer.Analyze(property_accessor);
            if (!property_access_tree.DoesEntireTreeSupportINotifyPropertyChangedAndChanging())
                throw new ArgumentException("All objects must implement INotifyPropertyChanged and INotifyPropertyChanging");

            if (property_change_subscribers == null)
                property_change_subscribers = new List<PropertyAccessTreeSubscriber<DependencyMethod<T>>>();

            // Build subscription tree from property access tree
            log.Trace("Creating subscription tree");
            var subscriber = property_access_tree.CreateSubscriptionTree<DependencyMethod<T>>(OnAnyPropertyInSubscriptionChanges);
            subscriber.DumpToLog();
            property_change_subscribers.Add(subscriber);

            return this;
        }

        private static void OnAnyPropertyInSubscriptionChanges(DependencyMethod<T> me, object object_that_changed)
        {
            me.callback((T)object_that_changed);
        }

        internal void SubscribeToChanges(INotifyPropertyChanged subject)
        {
            if (property_change_subscribers == null) return;

            foreach (var subscriber in property_change_subscribers)
                subscriber.SubscribeToChanges(subject, this);
        }

        internal void UnsubscribeFromChanges(INotifyPropertyChanged subject)
        {
            if (property_change_subscribers == null) return;

            foreach (var subscriber in property_change_subscribers)
                subscriber.UnsubscribeFromChanges(subject);
        }

        #region Graph debug output

        public IEnumerable<PropertyAccessTree> GraphDebug_GetPropertyAccessTrees()
        {
            if (property_change_subscribers == null)
                throw new Exception("Could not find a property access tree");

            return property_change_subscribers.Select(subscriber => subscriber.GraphDebug_GetPropertyAccessTree()).ToList();
        }

        #endregion
    }
}
