using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using CB.Xaml.ValueConverters;


namespace CB.Xaml.AttachedProperties
{
    public static class ItemsControlServices
    {
        #region Fields
        public static readonly DependencyProperty ActiveSelectionColorProperty =
            DependencyProperty.RegisterAttached("ActiveSelectionColor", typeof(Color), typeof(ItemsControlServices),
                new PropertyMetadata(SystemColors.HighlightColor, null, CoerceActiveSelectionColor));

        public static readonly DependencyProperty ActiveSelectionTextColorProperty =
            DependencyProperty.RegisterAttached("ActiveSelectionTextColor", typeof(Color),
                typeof(ItemsControlServices),
                new PropertyMetadata(SystemColors.HighlightTextColor, null, CoerceActiveSelectionTextColor));

        public static readonly DependencyProperty InactiveSelectionColorProperty =
            DependencyProperty.RegisterAttached("InactiveSelectionColor", typeof(Color), typeof(ItemsControlServices),
                new PropertyMetadata(SystemColors.InactiveSelectionHighlightBrush.Color, null,
                    CoerceInactiveSelectionColor));

        public static readonly DependencyProperty InactiveSelectionTextColorProperty =
            DependencyProperty.RegisterAttached("InactiveSelectionTextColor", typeof(Color),
                typeof(ItemsControlServices),
                new PropertyMetadata(SystemColors.InactiveSelectionHighlightTextBrush.Color, null,
                    CoerceInactiveSelectionTextColor));

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly",
            typeof(bool), typeof(ItemsControlServices), new PropertyMetadata(false, null, CoerceIsReadOnly));

        private static readonly List<DependencyObject> _actBrushObjects = new List<DependencyObject>();
        private static readonly List<DependencyObject> _actTextObjects = new List<DependencyObject>();
        private static readonly List<DependencyObject> _inaBrushObjects = new List<DependencyObject>();
        private static readonly List<DependencyObject> _inaTextObjects = new List<DependencyObject>();
        private static readonly List<DependencyObject> _isRolObjects = new List<DependencyObject>();
        #endregion


        #region Dependency Properties
        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static Color GetActiveSelectionColor(DependencyObject obj)
        {
            return (Color)obj.GetValue(ActiveSelectionColorProperty);
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static void SetActiveSelectionColor(DependencyObject obj, Color value)
        {
            obj.SetValue(ActiveSelectionColorProperty, value);
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static Color GetActiveSelectionTextColor(DependencyObject obj)
        {
            return (Color)obj.GetValue(ActiveSelectionTextColorProperty);
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static void SetActiveSelectionTextColor(DependencyObject obj, Color value)
        {
            obj.SetValue(ActiveSelectionTextColorProperty, value);
        }

        private static object CoerceActiveSelectionColor(DependencyObject d, object baseValue)
        {
            CoerceColor(d, baseValue, SystemColors.HighlightBrushKey, _actBrushObjects);
            return baseValue;
        }

        private static object CoerceActiveSelectionTextColor(DependencyObject d, object baseValue)
        {
            CoerceColor(d, baseValue, SystemColors.HighlightTextBrushKey, _actTextObjects);
            return baseValue;
        }

        private static object CoerceInactiveSelectionColor(DependencyObject d, object baseValue)
        {
            CoerceColor(d, baseValue, SystemColors.InactiveSelectionHighlightBrushKey, _inaBrushObjects);
            return baseValue;
        }

        private static object CoerceInactiveSelectionTextColor(DependencyObject d, object baseValue)
        {
            CoerceColor(d, baseValue, SystemColors.InactiveSelectionHighlightTextBrushKey, _inaTextObjects);
            return baseValue;
        }

        private static object CoerceIsReadOnly(DependencyObject d, object baseValue)
        {
            if (!_isRolObjects.Contains(d) && d is ItemsControl)
            {
                var itemCtrl = (ItemsControl)d;
                ResourceDictionary res;
                Style itemStyle;
                Type itemType;
                GetInfos(itemCtrl, out res, out itemStyle, out itemType);

                if (itemStyle != null && !itemStyle.IsSealed)
                {
                    var hittestVisibleSetter =
                        itemStyle.Setters.OfType<Setter>()
                                 .FirstOrDefault(s => s.Property == UIElement.IsHitTestVisibleProperty);
                    if (hittestVisibleSetter != null)
                    {
                        itemStyle.Setters.Remove(hittestVisibleSetter);
                    }
                    AddIsReadOnlyResource(itemCtrl, itemStyle);
                }
                else
                {
                    itemStyle = new Style(itemType);
                    AddIsReadOnlyResource(itemCtrl, itemStyle);
                    res.Add(itemType, itemStyle);
                }
                _isRolObjects.Add(d);
            }
            return baseValue;
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static Color GetInactiveSelectionColor(DependencyObject obj)
        {
            return (Color)obj.GetValue(InactiveSelectionColorProperty);
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static void SetInactiveSelectionColor(DependencyObject obj, Color value)
        {
            obj.SetValue(InactiveSelectionColorProperty, value);
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static Color GetInactiveSelectionTextColor(DependencyObject obj)
        {
            return (Color)obj.GetValue(InactiveSelectionTextColorProperty);
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static void SetInactiveSelectionTextColor(DependencyObject obj, Color value)
        {
            obj.SetValue(InactiveSelectionTextColorProperty, value);
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static bool GetIsReadOnly(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsReadOnlyProperty);
        }

        [Category("ItemsControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static void SetIsReadOnly(DependencyObject obj, bool value)
        {
            obj.SetValue(IsReadOnlyProperty, value);
        }
        #endregion


        #region Implementation
        private static void AddIsReadOnlyResource(ItemsControl d, Style itemStyle)
        {
            var hittestVisibleSetter = new Setter
            {
                Property = UIElement.IsHitTestVisibleProperty,
                Value = new Binding
                {
                    Source = d,
                    Path = new PropertyPath(IsReadOnlyProperty),
                    Converter = new BooleanNotConverter()
                }
            };
            itemStyle.Setters.Add(hittestVisibleSetter);
        }

        private static void CoerceColor(DependencyObject d, object baseValue, ResourceKey brushKey,
            IList<DependencyObject> colorBoundObjects)
        {
            if (d is ItemsControl && baseValue is Color)
            {
                var itemCtrl = (ItemsControl)d;
                var res = itemCtrl.Resources;

                if (!res.IsReadOnly)
                {
                    var color = (Color)baseValue;
                    var brush = new SolidColorBrush(color);
                    res[brushKey] = brush;
                }

                colorBoundObjects.Add(d);
            }
        }

        private static void GetInfos(ItemsControl itemCtrl, out ResourceDictionary res, out Style lbItemStyle,
            out Type itemType)
        {
            res = itemCtrl.Resources;

            if (itemCtrl is DataGrid)
            {
                itemType = typeof(DataGridRow);
                lbItemStyle = res.Values.OfType<Style>().FirstOrDefault(s => s.TargetType == typeof(DataGridRow));
            }
            else if (itemCtrl is ComboBox)
            {
                itemType = typeof(ComboBoxItem);
                lbItemStyle = res.Values.OfType<Style>().FirstOrDefault(s => s.TargetType == typeof(ComboBoxItem));
            }
            else if (itemCtrl is ListView)
            {
                itemType = typeof(ListViewItem);
                lbItemStyle = res.Values.OfType<Style>().FirstOrDefault(s => s.TargetType == typeof(ListViewItem));
            }
            else
            {
                itemType = typeof(ListBoxItem);
                lbItemStyle = res.Values.OfType<Style>().FirstOrDefault(s => s.TargetType == typeof(ListBoxItem));
            }

            var targetType = itemType;
            lbItemStyle = res.Values.OfType<Style>().FirstOrDefault(s => s.TargetType == targetType);
        }
        #endregion
    }
}