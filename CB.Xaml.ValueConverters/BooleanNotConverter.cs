using System;
using System.Globalization;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    // Convert true to false and vice versa
    [ValueConversion(typeof(bool), typeof(bool)), ValueConversion(typeof(bool?), typeof(bool?)),
     ValueConversion(typeof(bool), typeof(bool?)), ValueConversion(typeof(bool?), typeof(bool))]
    public class BooleanNotConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DoConvert(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DoConvert(value, targetType);
        }
        #endregion


        #region Implementation
        private static object DoConvert(object value, Type targetType)
        {
            if (value is bool && (targetType == typeof(bool) || targetType == typeof(bool?)))
            {
                return !(bool)value;
            }

            if (value is bool? && targetType == typeof(bool?))
            {
                return !(bool?)value;
            }

            throw new NotSupportedException();
        }
        #endregion
    }
}