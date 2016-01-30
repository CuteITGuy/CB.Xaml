using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Humanizer;


namespace CB.Xaml.ValueConverters.Impl
{
    public static class EnumConverter
    {
        #region Methods
        public static object ConvertFromString(object value, Type enumType, CultureInfo culture)
        {
            if (!(value is string) || enumType == null || !enumType.IsEnum)
            {
                return DependencyProperty.UnsetValue;
            }

            var stringValue = (string)value;
            var enumValue =
                Enum.GetValues(enumType)
                    .Cast<Enum>()
                    .FirstOrDefault(
                        v =>
                        string.Equals(v.ToString(), stringValue, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(v.Humanize(), stringValue, StringComparison.OrdinalIgnoreCase));

            return enumValue ?? DependencyProperty.UnsetValue;
        }

        public static object ConvertToString(object value, Type enumType, CultureInfo culture, LetterCasing letterCasing)
        {
            return value != null ? value.ToString().Humanize(letterCasing) : DependencyProperty.UnsetValue;
        }
        #endregion
    }
}