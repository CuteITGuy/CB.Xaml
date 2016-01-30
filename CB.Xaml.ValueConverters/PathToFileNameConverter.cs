using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    /// <summary>
    ///     Represents the converter that extracts the file name part from a file path.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class PathToFileNameConverter: IValueConverter
    {
        #region Methods
        /// <summary>
        ///     Extracts the file name part of a file path.
        /// </summary>
        /// <param name="value">Specifies the file path. This must be a string value.</param>
        /// <param name="targetType">Any type that can be converted from a string value.</param>
        /// <param name="parameter">
        ///     Can be null, but if provided, should be a boolean value, specifies whether the file name part
        ///     contains the extension part (true) or not (false).
        /// </param>
        /// <param name="culture">Not used.</param>
        /// <returns>The file name part.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            if (parameter is bool && !(bool)parameter)
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            return Path.GetFileName(path);
        }

        /// <summary>
        ///     Not used.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
}