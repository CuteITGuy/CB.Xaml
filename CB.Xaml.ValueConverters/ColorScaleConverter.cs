using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace CB.Xaml.ValueConverters
{
    public class ColorScaleConverter: IValueConverter
    {
        #region Fields
        #endregion


        #region  Properties & Indexers
        public Color Color { get; set; } = Colors.Red;

        public double Scale { get; set; } = 100.0;

        public ScaleOption ScaleOption { get; set; } = ScaleOption.ScaleRgb;
        #endregion


        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ratio = System.Convert.ToDouble(value) / Scale;

            if (ratio > 1.0 || ratio < 0.0)
            {
                throw new ArgumentException("value must lie between 0 and Scale.");
            }

            var dif = 255 * (1 - ratio);
            byte a = Color.A, r = Color.R, g = Color.G, b = Color.B;

            switch (ScaleOption)
            {
                case ScaleOption.ScaleAlphaOnly:
                    a = System.Convert.ToByte(ratio * 255);
                    break;
                case ScaleOption.ScaleRedOnly:
                    g = CalculateComponent(ratio, dif, g);
                    b = CalculateComponent(ratio, dif, b);
                    break;
                case ScaleOption.ScaleGreenOnly:
                    b = CalculateComponent(ratio, dif, b);
                    r = CalculateComponent(ratio, dif, r);
                    break;
                case ScaleOption.ScaleBlueOnly:
                    r = CalculateComponent(ratio, dif, r);
                    g = CalculateComponent(ratio, dif, g);
                    break;
                case ScaleOption.ScaleRgb:
                    r = CalculateComponent(ratio, dif, r);
                    g = CalculateComponent(ratio, dif, g);
                    b = CalculateComponent(ratio, dif, b);
                    break;
                default:
                    throw new NotImplementedException();
            }

            var newColor = Color.FromArgb(a, r, g, b);

            if (targetType == typeof(Color))
            {
                return newColor;
            }

            if (targetType == typeof(Brush))
            {
                return new SolidColorBrush(newColor);
            }

            if (targetType == typeof(string))
            {
                var con = new ColorConverter();
                con.ConvertToString(null, culture, newColor);
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion


        #region Implementation
        private byte CalculateComponent(double ratio, double dif, byte component)
        {
            return System.Convert.ToByte(ratio * component + dif);
        }
        #endregion
    }
}