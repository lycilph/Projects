using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditableTextBlockV2
{
    public class ObservableViewModelCollection<TViewModel, TModel> : ObservableCollection<TViewModel> where TViewModel : IViewModel<TModel>
    {
        private readonly ObservableCollection<TModel> source;
        private readonly Func<TModel, TViewModel> view_model_factory;

        private bool updating_model_collection = false;
        private bool updating_viewmodel_collection = false;

        public ObservableViewModelCollection(ObservableCollection<TModel> source, Func<TModel, TViewModel> view_model_factory)
            : base(source.Select(model => view_model_factory(model)))
        {
            this.source = source;
            this.view_model_factory = view_model_factory;
            
            source.CollectionChanged += OnSourceCollectionChanged;
        }

        protected virtual TViewModel CreateViewModel(TModel model)
        {
            return view_model_factory(model);
        }
        protected virtual TViewModel CreateViewModel(object model)
        {
            return CreateViewModel((TModel)model);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (updating_viewmodel_collection) return;

            updating_model_collection = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                        source.Insert(e.NewStartingIndex + i, ((TViewModel)e.NewItems[i]).Model);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                        source.RemoveAt(e.OldStartingIndex);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported operation " + e.Action);
            }
            updating_model_collection = false;
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (updating_model_collection) return;

            updating_viewmodel_collection = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                        Insert(e.NewStartingIndex + i, CreateViewModel(e.NewItems[i]));
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldItems.Count == 1)
                    {
                        Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else
                    {
                        List<TViewModel> items = this.Skip(e.OldStartingIndex).Take(e.OldItems.Count).ToList();
                        for (int i = 0; i < e.OldItems.Count; i++)
                            RemoveAt(e.OldStartingIndex);

                        for (int i = 0; i < items.Count; i++)
                            Insert(e.NewStartingIndex + i, items[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                        RemoveAt(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // remove
                    for (int i = 0; i < e.OldItems.Count; i++)
                        RemoveAt(e.OldStartingIndex);

                    // add
                    for (int i = 0; i < e.NewItems.Count; i++)
                        Insert(e.NewStartingIndex + i, CreateViewModel(e.NewItems[i]));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    for (int i = 0; i < e.NewItems.Count; i++)
                        Add(CreateViewModel(e.NewItems[i]));
                    break;

                default:
                    break;
            }
            updating_viewmodel_collection = false;
        }
    }
}
