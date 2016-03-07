using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using CB.Media.Brushes.Impl;
using static System.Windows.Media.ColorConverter;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(Color), typeof(Color)),
     ValueConversion(typeof(string), typeof(Color)),
     ValueConversion(typeof(Color), typeof(Brush)),
     ValueConversion(typeof(string), typeof(Brush))]
    public class ColorToContrastColorConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the source color
            var color = GetSourceColor(value);

            object brightObject, darkObject;

            // Get the dark and bright objects if provided
            if (ExtractParameter(parameter, out brightObject, out darkObject))
            {
                return ColorHelper.GetContrastObject(color, brightObject, darkObject);
            }

            // If not, calculate the target color/brush
            if (targetType == typeof(Color))
            {
                return ColorHelper.GetContrastBlackWhiteColor(color);
            }

            if (targetType == typeof(Brush))
            {
                return ColorHelper.GetContrastBlackWhiteBrush(color);
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion


        #region Implementation
        private static bool ExtractParameter(object parameter, out object brightObject, out object darkObject)
        {
            if (parameter == null)
            {
                brightObject = darkObject = null;
                return false;
            }

            var enumerable = parameter as IEnumerable;

            if (enumerable != null)
            {
                var enumerator = enumerable.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    brightObject = enumerator.Current;
                    if (enumerator.MoveNext())
                    {
                        darkObject = enumerator.Current;
                        return true;
                    }
                    throw new ArgumentException();
                }
                throw new ArgumentException();
            }
            throw new ArgumentException();
        }

        private static Color GetSourceColor(object value)
        {
            if (value is Color)
            {
                return (Color)value;
            }

            var valueString = value as string;
            if (valueString != null)
            {
                var colorString = ConvertFromString(valueString);
                if (colorString != null) return (Color)colorString;
            }

            throw new NotSupportedException();
        }
        #endregion
    }
}