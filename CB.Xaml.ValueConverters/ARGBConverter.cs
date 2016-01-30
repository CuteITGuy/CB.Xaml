using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace CB.Xaml.ValueConverters
{
    /// <summary>
    ///     This class implements IMultiValueConverter to convert an array of numeral values to a Color or Brush or
    ///     String object.
    ///     <para>
    ///         Example: Using an ARGBConverter object combined with MultiBinding users can define the Fill brush
    ///         of a Rectangle from values of four Sliders each specifies one color component value (A, G, R or B).
    ///     </para>
    /// </summary>
    public class ArgbConverter: IMultiValueConverter
    {
        #region Methods
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Color.FromArgb(System.Convert.ToByte(values[0]), System.Convert.ToByte(values[1]),
                System.Convert.ToByte(values[2]), System.Convert.ToByte(values[3]));

            if (targetType == typeof(Color))
            {
                return color;
            }

            if (targetType == typeof(Brush))
            {
                return new SolidColorBrush(color);
            }

            if (targetType == typeof(string))
            {
                return (new ColorConverter()).ConvertToString(color);
            }

            return DependencyProperty.UnsetValue;
        }

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }
        #endregion
    }
}