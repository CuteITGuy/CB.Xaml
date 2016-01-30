using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    public class BooleanToObjectConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return ObjectWhenNull;
            }

            if (!(value is bool?))
            {
                return DependencyProperty.UnsetValue;
            }

            var nullableBool = (bool?)value;
            return nullableBool.Value ? ObjectWhenTrue : ObjectWhenFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (value == ObjectWhenNull)
            {
                return null;
            }

            if (value == ObjectWhenFalse)
            {
                return false;
            }

            if (value == ObjectWhenTrue)
            {
                return true;
            }

            return DependencyProperty.UnsetValue;
        }
        #endregion


        #region Properties
        public object ObjectWhenFalse { get; set; }

        public object ObjectWhenNull { get; set; }

        public object ObjectWhenTrue { get; set; }
        #endregion
    }
}