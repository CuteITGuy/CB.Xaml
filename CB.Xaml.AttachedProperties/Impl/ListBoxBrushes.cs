using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CB.Xaml.AttachedProperties.Impl
{
    public class ListBoxBrushes
    {
        #region Fields
        private ResourceDictionary _resDict;
        #endregion


        #region  Constructors & Destructors
        public ListBoxBrushes()
        {
            GetResources();
        }
        #endregion


        #region  Properties & Indexers
        public Brush MouseOverBackground { get; set; }

        public Brush MouseOverBorderBrush { get; set; }

        public Brush MouseOverForeground { get; set; }

        public Brush SelectedActiveBackground { get; set; }

        public Brush SelectedActiveBorderBrush { get; set; }

        public Brush SelectedActiveForeground { get; set; }

        public Brush SelectedInactiveBackground { get; set; }

        public Brush SelectedInactiveBorderBrush { get; set; }

        public Brush SelectedInactiveForeground { get; set; }
        #endregion


        #region Methods
        public void ApplyStyle(FrameworkElement targetElement)
        {
            var template = GetResourceTemplate();
            template.Triggers[0] = InitializeTrigger("MouseOver");
            template.Triggers[1] = InitializeTrigger("SelectedInactive");
            template.Triggers[2] = InitializeTrigger("SelectedActive");

            var style = GetResourceStyle();
            style.Setters.Add(new Setter(Control.TemplateProperty, template));
            targetElement.Resources[style.TargetType] = style;
        }
        #endregion


        #region Implementation
        private Brush GetBrushValue(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(MouseOverBackground):
                    return MouseOverBackground;

                case nameof(MouseOverBorderBrush):
                    return MouseOverBorderBrush;

                case nameof(MouseOverForeground):
                    return MouseOverForeground;

                case nameof(SelectedActiveBackground):
                    return SelectedActiveBackground;

                case nameof(SelectedActiveBorderBrush):
                    return SelectedActiveBorderBrush;

                case nameof(SelectedActiveForeground):
                    return SelectedActiveForeground;

                case nameof(SelectedInactiveBackground):
                    return SelectedInactiveBackground;

                case nameof(SelectedInactiveBorderBrush):
                    return SelectedInactiveBorderBrush;

                case nameof(SelectedInactiveForeground):
                    return SelectedInactiveForeground;

                default:
                    throw new NotImplementedException();
            }
        }

        private Brush GetResourceBrush(string brushKey)
        {
            return _resDict[brushKey] as Brush;
        }

        private static ResourceDictionary GetResourceDict()
        {
            var resDict = new ResourceDictionary
            {
                Source =
                    new Uri("/CB.Xaml.AttachedProperties;Component/Resources/ListBoxItem.xaml",
                        UriKind.RelativeOrAbsolute)
            };

            return resDict;
        }

        private void GetResources()
        {
            _resDict = GetResourceDict();
            InitializeBrushes();
        }

        private Style GetResourceStyle()
        {
            return _resDict["ListBoxItemStyle"] as Style;
        }

        private ControlTemplate GetResourceTemplate()
        {
            var template = _resDict["ListBoxItemTemplate"] as ControlTemplate;
            return template;
        }

        private MultiTrigger GetResourceTrigger(string triggerKey)
        {
            return _resDict[triggerKey] as MultiTrigger;
        }

        private static Setter GetTriggerSetter(MultiTrigger trigger, string property)
        {
            return trigger.Setters.OfType<Setter>().FirstOrDefault(s => s.Property.Name == property);
        }

        private void InitializeBrushes()
        {
            foreach (
                var brushProperty in
                    GetType().GetProperties().Where(p => p.PropertyType == typeof(Brush)))
            {
                brushProperty.SetValue(this, GetResourceBrush(brushProperty.Name));
            }
        }

        private MultiTrigger InitializeTrigger(string action)
        {
            var triggerName = action + "Trigger";
            var trigger = GetResourceTrigger(triggerName);
            SetTriggerBrush(trigger, action, "Background");
            SetTriggerBrush(trigger, action, "BorderBrush");
            SetTriggerBrush(trigger, action, "Foreground");
            return trigger;
        }

        private void SetTriggerBrush(MultiTrigger trigger, string action, string property)
        {
            var setter = GetTriggerSetter(trigger, property);
            var value = GetBrushValue(action + property);
            setter.Value = value;
        }
        #endregion
    }
}