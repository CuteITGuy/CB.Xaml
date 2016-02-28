using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    public class BooleanToValueConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return ValueIfNull;
            }

            if (!(value is bool?))
            {
                return DependencyProperty.UnsetValue;
            }

            var nullableBool = (bool?)value;
            return nullableBool.Value ? ValueIfTrue : ValueIfFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (value == ValueIfNull)
            {
                return null;
            }

            if (value == ValueIfFalse)
            {
                return false;
            }

            if (value == ValueIfTrue)
            {
                return true;
            }

            return DependencyProperty.UnsetValue;
        }
        #endregion


        #region Properties
        public object ValueIfFalse { get; set; }

        public object ValueIfNull { get; set; }

        public object ValueIfTrue { get; set; }
        #endregion
    }
}