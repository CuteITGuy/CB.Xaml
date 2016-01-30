using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(bool?), typeof(bool))]
    public class NullableBoolConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullableBool = value as bool?;

            if (nullableBool == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return (bool)nullableBool;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool?)(bool)value;
            }
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
}