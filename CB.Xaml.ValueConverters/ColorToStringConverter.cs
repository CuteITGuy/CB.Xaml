using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using CB.Xaml.ValueConverters.Impl;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToStringConverter: IValueConverter
    {
        #region  Properties & Indexers
        public ColorToStringFormat Format { get; set; } = ColorToStringFormat.Argb;
        #endregion


        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color)) return DependencyProperty.UnsetValue;
            return ConvertToString((Color)value, GetFormat(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorConverter = new ColorConverter();
            return colorConverter.IsValid(value) ? colorConverter.ConvertFrom(value) : DependencyProperty.UnsetValue;
        }
        #endregion


        #region Implementation
        private string ConvertToString(Color color, ColorToStringFormat format)
        {
            switch (format)
            {
                case ColorToStringFormat.Rgb:
                    return $"#{color.R:X2}{color.G:X2}{color.B:X2}";

                case ColorToStringFormat.Argb:
                    return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";

                case ColorToStringFormat.ScArgb:
                    return $"sc#{color.ScA}, {color.ScR}, {color.ScG}, {color.ScB}";

                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        private ColorToStringFormat GetFormat(object parameter)
        {
            return parameter as ColorToStringFormat? ?? Format;
        }
        #endregion
    }
}