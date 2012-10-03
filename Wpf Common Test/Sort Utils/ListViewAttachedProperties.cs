using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Linq;
using System.Collections.Specialized;

namespace SortUtils
{
    public static class ListViewAttachedProperties
    {
        private const double MinColumnWidth = 50;
        private const double ColumnPadding = 3;

        // Auto size column stuff
        public static bool GetIsLastColumn(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsLastColumnProperty);
        }

        public static void SetIsLastColumn(DependencyObject obj, bool value)
        {
            obj.SetValue(IsLastColumnProperty, value);
        }

        public static readonly DependencyProperty IsLastColumnProperty =
            DependencyProperty.RegisterAttached("IsLastColumn", typeof (bool), typeof (ListViewAttachedProperties), new UIPropertyMetadata(false));

        public static ListView GetParentListView(DependencyObject obj)
        {
            return (ListView) obj.GetValue(ParentListViewProperty);
        }

        public static void SetParentListView(DependencyObject obj, ListView value)
        {
            obj.SetValue(ParentListViewProperty, value);
        }

        public static readonly DependencyProperty ParentListViewProperty =
            DependencyProperty.RegisterAttached("ParentListView", typeof (ListView), typeof (ListViewAttachedProperties), new UIPropertyMetadata(null));

        public static bool GetAutoSizeColumns(DependencyObject obj)
        {
            return (bool) obj.GetValue(AutoSizeColumnsProperty);
        }

        public static void SetAutoSizeColumns(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSizeColumnsProperty, value);
        }

        public static readonly DependencyProperty AutoSizeColumnsProperty =
            DependencyProperty.RegisterAttached("AutoSizeColumns", typeof (bool), typeof (ListViewAttachedProperties), new UIPropertyMetadata(false, new PropertyChangedCallback( OnAutoSizeColumnsChanged)));

        private static void OnAutoSizeColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ListView list_view = obj as ListView;
            if (list_view == null)
                return;

            if ((bool) args.NewValue == true)
                XamlHelper.ExecuteOnLoaded(list_view, fe => UpdateAutoSizing(fe, true));
            else
                UpdateAutoSizing(list_view, false);
        }

        private static void UpdateAutoSizing(FrameworkElement fe, bool enable)
        {
            ListView list_view = fe as ListView;
            if (list_view == null)
                return;
            GridView grid_view = list_view.View as GridView;
            if (grid_view == null)
                return;

            if (enable)
            {
                AutoSizeColumns(list_view, true);
                UpdateIsLastColumn(list_view);
                list_view.SizeChanged += ListViewSizeChanged;
                list_view.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(ColumnHeaderResized), true);

                grid_view.Columns.CollectionChanged += GridViewColumnsCollectionChanged;
                foreach (var col in grid_view.Columns)
                    SetParentListView(col, list_view);
            }
            else
            {
                list_view.SizeChanged -= ListViewSizeChanged;
                list_view.RemoveHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(ColumnHeaderResized));
                grid_view.Columns.CollectionChanged -= GridViewColumnsCollectionChanged;
            }
        }

        private static void GridViewColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var columns = sender as IEnumerable<GridViewColumn>;
            if (columns != null)
            {
                if (e.Action == NotifyCollectionChangedAction.Move)
                {
                    var col = columns.First();
                    if (col != null)
                    {
                        var list_view = GetParentListView(col);
                        UpdateIsLastColumn(list_view);
                    }
                }
            }
        }

        private static void ColumnHeaderResized(object sender, DragDeltaEventArgs e)
        {
            var list_view = sender as ListView;
            if (list_view != null)
            {
                var grid_view = list_view.View as GridView;
                if (grid_view != null)
                {
                    var thumb = e.OriginalSource as Thumb;
                    if (thumb != null)
                    {
                        GridViewColumnHeader header = thumb.TemplatedParent as GridViewColumnHeader;
                        if (header != null)
                        {
                            if (header.Column.Width < MinColumnWidth)
                            {
                                header.Column.Width = MinColumnWidth;
                            }
                            else
                            {
                                // Distribute the change to the columns to the right
                                int header_index = grid_view.Columns.IndexOf(header.Column);
                                int columns_to_the_right = grid_view.Columns.Count - header_index - 1;
                                double used_width_left = grid_view.Columns.Where((col, index) => index <= header_index).Sum(col => col.Width);
                                double used_width_right = grid_view.Columns.Where((col, index) => index > header_index).Sum(col => col.Width);
                                double list_view_content_width = list_view.ContentWidth() - ColumnPadding * grid_view.Columns.Count;
                                double width_delta = list_view_content_width - used_width_left - used_width_right;
                                double width_per_column = width_delta/(double)columns_to_the_right;

                                double remainder = 0;
                                for (int i = header_index + 1; i < grid_view.Columns.Count; i++)
                                {
                                    var col = grid_view.Columns[i];
                                    var new_width = col.Width + width_per_column + remainder;
                                    if (new_width > 0)
                                    {
                                        col.Width = new_width;
                                        if (col.Width < MinColumnWidth)
                                        {
                                            remainder = col.Width - MinColumnWidth;
                                            col.Width = MinColumnWidth;
                                        }
                                    }
                                    else
                                    {
                                        remainder = width_delta;
                                    }
                                }

                                if (remainder < 0)
                                    header.Column.Width += remainder;
                            }
                        }
                    }
                }
            }
        }

        private static void ListViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe != null && fe.IsLoaded)
                AutoSizeColumns(fe, false);
        }

        private static void AutoSizeColumns(FrameworkElement fe, bool is_first_time)
        {
            ListView list_view = fe as ListView;
            if (list_view == null)
                return;
            GridView grid_view = list_view.View as GridView;
            if (grid_view == null)
                return;

            // Find width of listview (without scrollbar)
            double list_view_content_width = list_view.ContentWidth() - ColumnPadding * grid_view.Columns.Count;

            if (is_first_time)
            {
                // if this is the first time, just set all columns to the same width
                foreach (var col in grid_view.Columns)
                    col.Width = (list_view_content_width/(double) grid_view.Columns.Count);
            }
            else
            {
                // Distribute the width according to the relative size of the old width
                double old_total_width = grid_view.Columns.Sum(col => col.Width);

                double remainder = 0;
                foreach (var col in grid_view.Columns)
                {
                    col.Width = (col.Width/old_total_width)*list_view_content_width + remainder;
                    if (col.Width < MinColumnWidth)
                    {
                        remainder = col.Width - MinColumnWidth;
                        col.Width = MinColumnWidth;
                    }
                }
            }
        }

        private static void UpdateIsLastColumn(FrameworkElement fe)
        {
            ListView list_view = fe as ListView;
            if (list_view == null)
                return;

            var headers = FindVisualChildren<GridViewColumnHeader>(list_view).Where(header => header.Role == GridViewColumnHeaderRole.Normal).Reverse();
            foreach (var header in headers)
                SetIsLastColumn(header, false);
            SetIsLastColumn(headers.Last(), true);
        }


        // Sort stuff
        public static ListSortDirection? GetSortDirection(DependencyObject obj)
        {
            return (ListSortDirection?) obj.GetValue(SortDirectionProperty);
        }

        public static void SetSortDirection(DependencyObject obj, ListSortDirection? value)
        {
            obj.SetValue(SortDirectionProperty, value);
        }

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.RegisterAttached("SortDirection", typeof (ListSortDirection?), typeof (ListViewAttachedProperties), new UIPropertyMetadata(null));

        public static string GetSortProperty(DependencyObject obj)
        {
            return (string) obj.GetValue(SortPropertyProperty);
        }

        public static void SetSortProperty(DependencyObject obj, string value)
        {
            obj.SetValue(SortPropertyProperty, value);
        }

        public static readonly DependencyProperty SortPropertyProperty = DependencyProperty.RegisterAttached("SortProperty", typeof (string), typeof (ListViewAttachedProperties), new UIPropertyMetadata(null));

        public static ICommand GetSortCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(SortCommandProperty);
        }

        public static void SetSortCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SortCommandProperty, value);
        }

        public static readonly DependencyProperty SortCommandProperty = DependencyProperty.RegisterAttached("SortCommand", typeof (ICommand), typeof (ListViewAttachedProperties), new UIPropertyMetadata(null, new PropertyChangedCallback(OnSortCommandChanged)));

        private static void OnSortCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ListView list_view = obj as ListView;
            if (list_view == null)
                return;
            GridView grid_view = list_view.View as GridView;
            if (grid_view == null)
                return;

            bool has_old_value = (args.OldValue != null);
            bool has_new_value = (args.NewValue != null);
            if (has_old_value && !has_new_value)
            {
                list_view.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeaderClick));
                foreach (var column in grid_view.Columns)
                    ListViewAttachedProperties.SetSortCommand(column, (ICommand) args.OldValue);
            }
            if (!has_old_value && has_new_value)
            {
                list_view.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeaderClick));
                foreach (var column in grid_view.Columns)
                    ListViewAttachedProperties.SetSortCommand(column, (ICommand) args.NewValue);
            }
        }

        private static void ColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader header = e.OriginalSource as GridViewColumnHeader;
            if (header == null || header.Column == null || header.Role != GridViewColumnHeaderRole.Normal)
                return;

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
            {
                ListView list_view = sender as ListView;
                if (list_view != null)
                    ResetColumns(list_view, header);
            }

            // Set direction on the header
            ListSortDirection? old_sort_direction = GetSortDirection(header);
            ListSortDirection? new_sort_direction;

            if (old_sort_direction == ListSortDirection.Ascending)
                new_sort_direction = ListSortDirection.Descending;
            else if (old_sort_direction == ListSortDirection.Descending)
                new_sort_direction = null;
            else
                new_sort_direction = ListSortDirection.Ascending;

            UpdateHeader(header, new_sort_direction);
        }

        private static void ResetColumns(ListView list_view, GridViewColumnHeader header_to_except)
        {
            foreach (var header in FindVisualChildren<GridViewColumnHeader>(list_view))
                if (header.Column != null &&
                    header.Role == GridViewColumnHeaderRole.Normal &&
                    header != header_to_except &&
                    GetSortDirection(header).HasValue)
                    UpdateHeader(header, null);
        }

        private static void UpdateHeader(GridViewColumnHeader header, ListSortDirection? direction)
        {
            // Set the sort direction on the header
            SetSortDirection(header, direction);

            // Find the property to report to the command
            string property = GetSortProperty(header.Column);
            if (property == null)
            {
                if (header.Content is string)
                    property = header.Content as string;
                else
                    throw new InvalidOperationException("No sort property or string header defined on column");
            }

            SortArgument sort_argument = new SortArgument(property, direction);

            // Execute the command
            ICommand command = GetSortCommand(header.Column) as ICommand;
            if (command != null && command.CanExecute(sort_argument))
                command.Execute(sort_argument);
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                        yield return (T) child;

                    foreach (T child_of_child in FindVisualChildren<T>(child))
                        yield return child_of_child;
                }
            }
        }

        private static double ContentWidth(this ListView list_view)
        {
            // Find width of listview (without scrollbar)
            Decorator border = VisualTreeHelper.GetChild(list_view, 0) as Decorator;
            if (border != null)
            {
                ScrollViewer scroller = border.Child as ScrollViewer;
                if (scroller != null)
                {
                    ItemsPresenter presenter = scroller.Content as ItemsPresenter;
                    if (presenter != null)
                    {
                        return presenter.ActualWidth;
                    }
                }
            }
            return 0;
        }
    }
}
