using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(DateTime), typeof(double)),
     ValueConversion(typeof(Duration), typeof(double)),
     ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeToDoubleConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeConverter.ConvertTimeToDouble(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeConverter.ConvertDoubleToTime(value, targetType, parameter, culture);
        }
        #endregion
    }
}