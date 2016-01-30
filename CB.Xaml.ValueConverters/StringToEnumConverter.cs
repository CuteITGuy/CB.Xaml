using System;
using System.Globalization;
using System.Windows.Data;
using CB.Xaml.ValueConverters.Impl;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(string), typeof(Enum))]
    public class StringToEnumConverter: HumanizedStringConverterBase
    {
        #region Override
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumConverter.ConvertFromString(value, parameter as Type, culture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumConverter.ConvertToString(value, parameter as Type, culture, LetterCasing);
        }
        #endregion
    }
}