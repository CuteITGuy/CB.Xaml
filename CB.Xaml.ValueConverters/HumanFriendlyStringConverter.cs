using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Humanizer;
using EnumConverter = CB.Xaml.ValueConverters.Impl.EnumConverter;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(string), typeof(string)),
     ValueConversion(typeof(object), typeof(string))]
    public class HumanFriendlyStringConverter: HumanizedStringConverterBase
    {
        #region Override
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var stringValue = value as string ?? value.ToString();
                return stringValue.Humanize(_letterCasing);
            }
            return DependencyProperty.UnsetValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;

            if (stringValue == null)
            {
                return DependencyProperty.UnsetValue;
            }

            targetType = SourceType ?? targetType;

            if (targetType.IsEnum)
            {
                return EnumConverter.ConvertFromString(value, targetType, culture);
            }

            var dehumanizedString = DehumanizeString(stringValue);

            if (targetType == typeof(string))
            {
                return dehumanizedString;
            }

            var targetTypeConverter = TypeDescriptor.GetConverter(targetType);

            return targetTypeConverter.CanConvertFrom(typeof(string))
                       ? targetTypeConverter.ConvertFrom(dehumanizedString)
                       : DependencyProperty.UnsetValue;
        }
        #endregion
    }
}