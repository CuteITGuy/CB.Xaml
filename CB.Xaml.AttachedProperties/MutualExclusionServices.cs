using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;


namespace CB.Xaml.AttachedProperties
{
    public class MutualExclusionServices
    {
        [Category("MutualExclusionServices")]
        [AttachedPropertyBrowsableForType(typeof(ToggleButton))]
        [AttachedPropertyBrowsableForType(typeof(MenuItem))]
        public static string GetGroup(DependencyObject d)
        {
            return (string)d.GetValue(GroupProperty);
        }

        [Category("MutualExclusionServices")]
        [AttachedPropertyBrowsableForType(typeof(ToggleButton))]
        [AttachedPropertyBrowsableForType(typeof(MenuItem))]
        public static void SetGroup(DependencyObject d, string value)
        {
            d.SetValue(GroupProperty, value);
        }

        static MutualExclusionServices()
        {
            
        }


        public static readonly DependencyProperty GroupProperty = DependencyProperty.RegisterAttached(
            "Group", typeof(string), typeof(MutualExclusionServices),
            new PropertyMetadata("", OnGroupChanged));

        private static readonly Dictionary<string, List<DependencyObject>> _groups =
            new Dictionary<string, List<DependencyObject>>();

        private static void AddElement(DependencyObject element, string group)
        {
            if (!_groups.ContainsKey(group) || _groups[group] == null)
            {
                _groups[group] = new List<DependencyObject>();
            }
            _groups[group].Add(element);
            AddHandler(element);
        }

        private static void AddHandler(DependencyObject element)
        {
            if (element is ToggleButton) ((ToggleButton)element).Checked += Element_Checked;
            else if (element is MenuItem) ((MenuItem)element).Checked += Element_Checked;
        }

        

        private static bool OfValidType(DependencyObject obj)
        {
            return obj is ToggleButton || obj is MenuItem;
        }

        private static void RemoveElement(DependencyObject element, string group)
        {
            if (_groups.ContainsKey(group) && _groups[group].Contains(element))
            {
                _groups[group].Remove(element);
                RemoveHandler(element);
            }
        }

        private static void RemoveHandler(DependencyObject element)
        {
            if (element is ToggleButton) ((ToggleButton)element).Checked -= Element_Checked;
            else if (element is MenuItem) ((MenuItem)element).Checked -= Element_Checked;
        }

        private static void UnCheck(DependencyObject element)
        {
            if (element is ToggleButton) ((ToggleButton)element).IsChecked = false;
            else if (element is MenuItem) ((MenuItem)element).IsChecked = false;
        }

        private static void Element_Checked(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            var group = _groups.Values.FirstOrDefault(g => g.Contains(element));
            if (group == null) return;

            foreach (var item in group.Where(i => !Equals(i, element)))
            {
                UnCheck(item);
            }
        }

        private static void OnGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!OfValidType(d)) return;

            string newGroup = (string)e.NewValue, oldGroup = (string)e.OldValue;
            RemoveElement(d, oldGroup);
            if (!string.IsNullOrEmpty(newGroup))
            {
                AddElement(d, newGroup);
            }
        }
    }
}