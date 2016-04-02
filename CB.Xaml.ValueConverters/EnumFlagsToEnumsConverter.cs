using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(Enum), typeof(IEnumerable<Enum>))]
    public class EnumFlagsToEnumsConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum))
            {
                return DependencyProperty.UnsetValue;
            }

            var enumValue = (Enum)value;
            var enumType = value.GetType();
            var enums = Enum.GetValues(enumType).Cast<Enum>().Where(e => enumValue.HasFlag(e)).ToArray();
            return enums;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enums = value as IEnumerable<Enum>;

            if (enums == null)
            {
                return DependencyProperty.UnsetValue;
            }

            var enumArray = enums as Enum[] ?? enums.ToArray();
            if (enumArray.Length == 0)
            {
                return DependencyProperty.UnsetValue;
            }

            dynamic enumValue = enumArray[0];

            for (var i = 0; i < enumArray.Length; i++)
            {
                enumValue |= enumArray[i];
            }

            return enumValue;
        }
        #endregion
    }
}