using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;

namespace Wpf_toolkit_test
{
    // http://stackoverflow.com/questions/11505283/re-sort-wpf-datagrid-after-bounded-data-has-changed
    public static class DataGridAttachedProperties
    {
        public static ICommand GetCustomSortCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CustomSortCommandProperty);
        }
        public static void SetCustomSortCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CustomSortCommandProperty, value);
        }
        public static readonly DependencyProperty CustomSortCommandProperty =
            DependencyProperty.RegisterAttached("CustomSortCommand", typeof(ICommand), typeof(DataGridAttachedProperties), new UIPropertyMetadata(null, new PropertyChangedCallback(OnCustomSortCommandChanged)));

        private static void OnCustomSortCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // We only setup sorting for datagrids
            DataGrid dg = obj as DataGrid;
            if (dg == null)
                return;

            // Add an event handler for the sort event
            dg.Sorting += DataGridSorting;

            // Get the sort command
            ICommand command = GetCustomSortCommand(dg);

            foreach (var col in dg.Columns)
            {
                // Add an event handler to the SortDirection property
                PropertyDescriptor prop = TypeDescriptor.GetProperties(col)["SortDirection"];
                prop.AddValueChanged(col, SortDirectionChanged);

                // Add the CustomSortCommand to the columns also
                DataGridAttachedProperties.SetCustomSortCommand(col, command);
            }
        }

        private static void SortDirectionChanged(object sender, EventArgs args)
        {
            DataGridColumn column = sender as DataGridColumn;
            if (column == null)
                return;

            Sorter s = new Sorter(column.Header.ToString(), column.SortDirection);

            // Execute command
            ICommand command = GetCustomSortCommand(column);
            if (command != null && command.CanExecute(s))
                command.Execute(s);
        }

        private static void DataGridSorting(object sender, DataGridSortingEventArgs args)
        {
            DataGridColumn column = args.Column;
            if (column.SortDirection == null)
                column.SortDirection = ListSortDirection.Ascending;
            else if (column.SortDirection == ListSortDirection.Ascending)
                column.SortDirection = ListSortDirection.Descending;
            else
                column.SortDirection = null;

            args.Handled = true;
        }
    }
}
