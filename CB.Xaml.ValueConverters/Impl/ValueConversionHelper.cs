using System;
using System.ComponentModel;


namespace CB.Xaml.ValueConverters.Impl
{
    public class ValueConversionHelper
    {
        #region Methods
        public static object TryConvertFrom(object value, Type toType)
        {
            if (value == null) return null;
            var typeConverter = TypeDescriptor.GetConverter(toType);
            return typeConverter.IsValid(value) ? typeConverter.ConvertFrom(value) : null;
        }

        public static object TryGetValue(object value, Type destinationType)
        {
            if (value == null) return null;
            var valueType = value.GetType();
            if (valueType == destinationType || valueType.IsSubclassOf(destinationType)) return value;
            if (destinationType == typeof(string)) return value.ToString();
            return TryConvertFrom(value, destinationType);
        }

        public static bool TryGetValue<T>(object value, out T result)
        {
            var tryGetValue = TryGetValue(value, typeof(T));
            if (tryGetValue == null)
            {
                result = default(T);
                return false;
            }
            result = (T)tryGetValue;
            return true;
        }
        #endregion
    }
}