using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    public class NullityConverter: IValueConverter
    {
        #region  Properties & Indexers
        public object ValueWhenNotNull { get; set; } = DependencyProperty.UnsetValue;
        public object ValueWhenNull { get; set; } = DependencyProperty.UnsetValue;
        #endregion


        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? ValueWhenNull : ValueWhenNotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
}