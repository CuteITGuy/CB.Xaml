using System;
using System.Globalization;


namespace CB.Xaml.ValueConverters
{
    /// <summary>
    ///     This class implements IMultiValueConverter to convert an array of numeral values to a Color or Brush or
    ///     String object.
    ///     <para>
    ///         Example: Using an RGBConverter object combined with MultiBinding users can define the Fill brush
    ///         of a Rectangle from values of three Sliders each specifies one color component value (G, R or B).
    ///     </para>
    /// </summary>
    public class RgbConverter: ArgbConverter
    {
        #region Override
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var values4 = new object[4];
            values4[0] = 255;
            values4[1] = values[0];
            values4[2] = values[1];
            values4[3] = values[2];
            return base.Convert(values4, targetType, parameter, culture);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }
        #endregion
    }
}