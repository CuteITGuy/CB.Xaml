using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static CB.Xaml.ValueConverters.Impl.ValueConversionHelper;


namespace CB.Xaml.ValueConverters
{
    public class AlphaColorConverter: IMultiValueConverter
    {
        #region Fields
        private const int BINDING_COUNT = 2;
        #endregion


        #region Methods
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte alpha;
            Color color;
            if (values == null || values.Length < BINDING_COUNT ||
                !TryGetValue(values[0], out alpha) || !TryGetValue(values[1], out color))
                return DependencyProperty.UnsetValue;

            color.A = alpha;
            return color;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Color color;
            if (!TryGetValue(value, out color)) return new object[0];

            var alpha = color.A;
            color.A = byte.MaxValue;

            var result = new object[targetTypes.Length];
            if (result.Length > 0) result[0] = TryGetValue(alpha, targetTypes[0]);
            if (result.Length > 1) result[1] = TryGetValue(color, targetTypes[1]);

            return result;
        }
        #endregion
    }
}