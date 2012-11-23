using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MVVM.Observable;
using NLog;
using System.ComponentModel;
using System.Collections;

namespace MVVM.Expressions
{
    public class ReactiveCollection<T> : IReactiveCollection<T>
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private Action notification_callback;

        private INotifyCollectionChanged original_collection;
        private readonly ObservableCollection<ItemViewModel> wrapped_collection = new ObservableCollection<ItemViewModel>();
        private readonly Dictionary<T, ItemViewModel> original_to_wrapped_item_map = new Dictionary<T, ItemViewModel>();
        private readonly List<Action<T, ItemViewModel>> item_transformations = new List<Action<T, ItemViewModel>>();

        public ObservableCollection<ItemViewModel> WrappedCollection
        {
            get { return wrapped_collection; }
        }

        public ReactiveCollection()
        {
            item_transformations.Add((item, model) => model.AllProperties(item));
        }

        public IReactiveCollection<T> Each(Action<T, ItemViewModel> transformation)
        {
            item_transformations.Add(transformation);
            UnsubscribeFromAllItems();
            SubscribeToAllItems();
            return this;
        }

        #region Subscription

        private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            log.Trace("ReactiveCollectionChanged " + args.Action);

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SubscribeToItems(args.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UnsubscribeFromItems(args.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    UnsubscribeFromItems(args.OldItems);
                    SubscribeToItems(args.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    // Do nothing here
                    break;
                case NotifyCollectionChangedAction.Reset:
                    UnsubscribeFromAllItems();
                    SubscribeToAllItems();
                    break;
                default:
                    throw new NotImplementedException("Action " + args.Action + " not implemented");
            }

            // Invoke notification callback
            notification_callback();
        }

        private void ElementChanged(object sender, PropertyChangedEventArgs args)
        {
            log.Trace("ReactiveCollectionElementChanged " + args.PropertyName);

            // Invoke notification callback
            notification_callback();
        }

        public void Subscribe(INotifyCollectionChanged subject, Action callback)
        {
            log.Trace("Subscribe reactive collection");

            notification_callback = callback;

            original_collection = subject;
            SubscribeToAllItems();
            original_collection.CollectionChanged += OriginalCollectionChanged;
        }

        public void Unsubscribe()
        {
            log.Trace("Unsubscribe reactive collection");

            notification_callback = null;

            original_collection.CollectionChanged -= OriginalCollectionChanged;
            UnsubscribeFromAllItems();
            original_collection = null;
        }

        private void SubscribeToAllItems()
        {
            var collection_as_enumerable = original_collection as IEnumerable<T>;
            if (collection_as_enumerable == null)
                throw new Exception("Subject must implement IEnumerable");

            foreach (var item in collection_as_enumerable)
                SubscribeToItem(item);
        }

        private void UnsubscribeFromAllItems()
        {
            foreach (var item_view_model in wrapped_collection)
            {
                // Unsubscribe from all events
                item_view_model.Unsubscribe();
                // Remove collection notification
                item_view_model.PropertyChanged -= ElementChanged;
            }
            original_to_wrapped_item_map.Clear();
            wrapped_collection.Clear();
        }

        private void SubscribeToItems(IList items)
        {
            foreach (var item in items)
                SubscribeToItem((T)item);
        }

        private void UnsubscribeFromItems(IList items)
        {
            foreach (var item in items)
                UnsubscribeFromItem((T)item);
        }

        private void SubscribeToItem(T item)
        {
            // Create view model
            var item_view_model = new ItemViewModel();
            wrapped_collection.Add(item_view_model);
            original_to_wrapped_item_map.Add(item, item_view_model);

            // Apply all transformation
            foreach (var transformation in item_transformations)
                transformation(item, item_view_model);

            // Add collection notification
            item_view_model.PropertyChanged += ElementChanged;
        }

        private void UnsubscribeFromItem(T item)
        {
            ItemViewModel item_view_model;
            if (original_to_wrapped_item_map.TryGetValue(item, out item_view_model))
            {
                // Unsubscribe from all events
                item_view_model.Unsubscribe();

                // Remove view model and mapping
                wrapped_collection.Remove(item_view_model);
                original_to_wrapped_item_map.Remove(item);

                // Remove collection notification
                item_view_model.PropertyChanged -= ElementChanged;
            }
        }

        #endregion
    }
}
