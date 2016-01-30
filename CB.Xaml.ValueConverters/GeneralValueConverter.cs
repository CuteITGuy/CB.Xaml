using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CB.Xaml.ValueConverters.Impl;


namespace CB.Xaml.ValueConverters
{
    public class GeneralValueConverter: IValueConverter
    {
        #region  Properties & Indexers
        public GeneralDictionary ConvertBackMapping { get; set; } = new GeneralDictionary();

        public GeneralDictionary ConvertMapping { get; set; } = new GeneralDictionary();

        public object DefaulConvertBackResult { get; set; } = DependencyProperty.UnsetValue;

        public object DefaultConvertResult { get; set; } = DependencyProperty.UnsetValue;
        #endregion


        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ConvertMapping != null && ConvertMapping.ContainsKey(value)
                   ? ConvertMapping[value] : DefaultConvertResult;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => ConvertBackMapping != null && ConvertBackMapping.ContainsKey(value)
                   ? ConvertBackMapping[value] : DefaulConvertBackResult;
        #endregion
    }
}