using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CB.Xaml.AttachedProperties.Impl;


namespace CB.Xaml.AttachedProperties
{
    public class ListBoxServices
    {
        #region Fields
        private static readonly IList<ListBox> _listBoxs = new List<ListBox>();
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
            "SelectedItems", typeof(IEnumerable), typeof(ListBoxServices),
            new PropertyMetadata(default(IEnumerable), OnSelectedItemsChanged));

        public static readonly DependencyProperty BrushesProperty = DependencyProperty
            .RegisterAttached(
                "Brushes", typeof(ListBoxBrushes), typeof(ListBoxServices),
                new PropertyMetadata(default(ListBoxBrushes), OnBrushesChanged));

        public static ListBoxBrushes GetBrushes(DependencyObject element)
        {
            return (ListBoxBrushes)element.GetValue(BrushesProperty);
        }

        public static void SetBrushes(DependencyObject element, ListBoxBrushes value)
        {
            element.SetValue(BrushesProperty, value);
        }

        [Category("ListBoxServices")]
        [AttachedPropertyBrowsableForType(typeof(ListBox))]
        public static IEnumerable GetSelectedItems(DependencyObject d)
        {
            return (IEnumerable)d.GetValue(SelectedItemsProperty);
        }

        [Category("ListBoxServices")]
        [AttachedPropertyBrowsableForType(typeof(ListBox))]
        public static void SetSelectedItems(DependencyObject d, IEnumerable value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }
        #endregion


        #region Event Handlers
        private static void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var element = sender as ListBox;
            element?.SetValue(SelectedItemsProperty, element.SelectedItems);
        }
        #endregion


        #region Implementation
        private static void OnBrushesChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var targetElement = d as FrameworkElement;
            var brushes = e.NewValue as ListBoxBrushes;
            if (targetElement != null)
            {
                brushes?.ApplyStyle(targetElement);
            }
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as ListBox;
            if (element == null) return;

            element.SelectedItems.Clear();
            var selectedItems = e.NewValue as IEnumerable;
            if (selectedItems == null) return;

            foreach (var item in
                from object item in element.Items
                from selectedItem in selectedItems.Cast<object>().Where(selectedItem => Equals(item, selectedItem))
                select item)
            {
                element.SelectedItems.Add(item);
            }

            if (!_listBoxs.Contains(element))
            {
                element.SelectionChanged += ListBoxOnSelectionChanged;
                _listBoxs.Add(element);
            }
        }
        #endregion
    }
}