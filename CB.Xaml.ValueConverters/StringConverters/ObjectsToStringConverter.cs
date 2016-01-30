using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters.StringConverters
{
    [ValueConversion(typeof(IEnumerable<object>), typeof(string))]
    public class ObjectsToStringConverter: IValueConverter
    {
        #region Fields
        private const string DEFAULT_SEPARATOR = " ";
        #endregion


        #region  Properties & Indexers
        public string Separator { get; set; }
        #endregion


        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var objects = value as IEnumerable<object>;
            if (objects == null) return DependencyProperty.UnsetValue;
            var separator = GetSeparator(parameter);
            return string.Join(separator, objects);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (s == null || (targetType != typeof(IEnumerable<string>) && targetType != typeof(string[])))
                return DependencyProperty.UnsetValue;

            return s.Split(new[] { GetSeparator(parameter) }, StringSplitOptions.None);
        }
        #endregion


        #region Implementation
        private string GetSeparator(object parameter)
        {
            return parameter as string ?? Separator ?? DEFAULT_SEPARATOR;
        }
        #endregion
    }
}