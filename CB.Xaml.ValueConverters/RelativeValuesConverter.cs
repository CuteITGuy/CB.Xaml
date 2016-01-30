using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using CB.Xaml.ValueConverters.Impl;


namespace CB.Xaml.ValueConverters
{
    public class RelativeValuesConverter: IMultiValueConverter
    {
        #region  Properties & Indexers
        public RelativeOperation Operation { get; set; }
        #endregion


        #region Methods
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var operation = GetOperation(parameter);
            switch (operation)
            {
                case RelativeOperation.First:
                    return values.FirstOrDefault();

                case RelativeOperation.Last:
                    return values.LastOrDefault();

                case RelativeOperation.Single:
                    return values.SingleOrDefault();

                case RelativeOperation.Min:
                    return values.Min();

                case RelativeOperation.Max:
                    return values.Max();

                case RelativeOperation.Sum:
                    return Sum(values);

                case RelativeOperation.Average:
                    return Average(values);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion


        #region Implementation
        private static object Average(object[] values)
            => values == null || values.Length == 0 || !IsNumericValues(values)
                   ? DependencyProperty.UnsetValue : CalculateDynamicSum(values) / values.Length;

        private static dynamic CalculateDynamicSum(IReadOnlyList<object> values)
        {
            dynamic sum = values[0];

            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < values.Count; i++)
            {
                sum += values[i];
            }
            return sum;
        }

        private RelativeOperation GetOperation(object parameter) => parameter as RelativeOperation? ?? Operation;

        private static bool IsNumericValues(IEnumerable<object> values)
            => values.All(v => v is byte || v is sbyte || v is short || v is ushort || v is int || v is uint ||
                               v is long || v is ulong || v is float || v is double || v is decimal);

        private static object Sum(object[] values) => values == null || values.Length == 0
                                                          ? DependencyProperty.UnsetValue
                                                          : (IsNumericValues(values)
                                                                 ? CalculateDynamicSum(values)
                                                                 : string.Concat(values));
        #endregion
    }
}