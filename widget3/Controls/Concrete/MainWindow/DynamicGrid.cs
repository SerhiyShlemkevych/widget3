using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace widget3.Controls.Concrete.MainWindow
{
    public class DynamicGrid
    {
        public static readonly DependencyProperty CellSizeProperty = 
            DependencyProperty.RegisterAttached(
            "CellSize", typeof(int), typeof(DynamicGrid),
            new PropertyMetadata(-1, CellSizeChanged));

        public static void CellSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            RowCountChanged(obj, new DependencyPropertyChangedEventArgs(DynamicGrid.RowCountProperty, DynamicGrid.GetRowCount(obj), DynamicGrid.GetRowCount(obj)));
            ColumnCountChanged(obj, new DependencyPropertyChangedEventArgs(DynamicGrid.RowCountProperty, DynamicGrid.GetRowCount(obj), DynamicGrid.GetRowCount(obj)));
        }

        // Get
        public static int GetCellSize(DependencyObject obj)
        {
            return (int)obj.GetValue(CellSizeProperty);
        }

        // Set
        public static void SetCellSize(DependencyObject obj, int value)
        {
            obj.SetValue(CellSizeProperty, value);
        }

        #region RowCount Property

        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.RegisterAttached(
                "RowCount", typeof(int), typeof(DynamicGrid),
                new PropertyMetadata(-1, RowCountChanged));


        // Get
        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        // Set
        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        // Change Event - Adds the Rows
        public static void RowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || (int)e.NewValue < 0)
                return;

            Grid grid = (Grid)obj;
            grid.RowDefinitions.Clear();

            var size = new GridLength((int)obj.GetValue(DynamicGrid.CellSizeProperty));

            for (int i = 0; i < (int)e.NewValue; i++)
                grid.RowDefinitions.Add(
                    new RowDefinition() { Height = size });
        }

        #endregion

        #region ColumnCount Property

        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.RegisterAttached(
                "ColumnCount", typeof(int), typeof(DynamicGrid),
                new PropertyMetadata(-1, ColumnCountChanged));

        // Get
        public static int GetColumnCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        // Set
        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }

        // Change Event - Add the Columns
        public static void ColumnCountChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || (int)e.NewValue < 0)
                return;

            Grid grid = (Grid)obj;
            grid.ColumnDefinitions.Clear();

            var size = new GridLength((int)obj.GetValue(DynamicGrid.CellSizeProperty));

            for (int i = 0; i < (int)e.NewValue; i++)
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = size });
        }

        #endregion 
    }
}
