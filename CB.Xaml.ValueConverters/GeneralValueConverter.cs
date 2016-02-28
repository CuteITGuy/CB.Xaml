using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using CB.Xaml.ValueConverters.Impl;


namespace CB.Xaml.ValueConverters
{
    [ContentProperty(nameof(ConvertMapping))]
    public class GeneralValueConverter: IValueConverter
    {
        #region  Properties & Indexers
        public GeneralDictionary ConvertBackMapping { get; set; } = new GeneralDictionary();

        public GeneralDictionary ConvertMapping { get; set; } = new GeneralDictionary();

        public object DefaulConvertBackResult { get; set; } = DependencyProperty.UnsetValue;

        public object DefaultConvertResult { get; set; } = DependencyProperty.UnsetValue;

        public object NullConvertBackResult { get; set; } = DependencyProperty.UnsetValue;

        public object NullConvertResult { get; set; } = DependencyProperty.UnsetValue;
        #endregion


        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null
                   ? NullConvertResult
                   : ConvertMapping != null && ConvertMapping.ContainsKey(value)
                         ? ConvertMapping[value]
                         : DefaultConvertResult;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null
                   ? NullConvertBackResult
                   : ConvertBackMapping != null && ConvertBackMapping.ContainsKey(value)
                         ? ConvertBackMapping[value]
                         : DefaulConvertBackResult;
        #endregion
    }
}