using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(string), typeof(string[]))]
    [ContentProperty("SplitSeparator")]
    public class SplitStringConverter: IValueConverter
    {
        #region Fields
        private static readonly string defaultSeparator = Environment.NewLine;
        #endregion


        #region  Properties & Indexers
        public string JoinSeparator { get; set; }

        public StringSplitOptions SplitOptions { get; set; }

        public StringCollection SplitSeparator { get; } = new StringCollection();
        #endregion


        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            return text == null ? DependencyProperty.UnsetValue : ConvertToLines(text);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lines = value as IEnumerable<string>;
            return lines == null ? DependencyProperty.UnsetValue : ConvertToText(lines);
        }
        #endregion


        #region Implementation
        private string[] ConvertToLines(string text)
        {
            return text.Split(GetSplitSeparator(), SplitOptions);
        }

        private string ConvertToText(IEnumerable<string> lines)
        {
            return string.Join(GetJoinSeparator(), lines);
        }

        private string GetJoinSeparator()
        {
            return JoinSeparator ?? defaultSeparator;
        }

        private string[] GetSplitSeparator()
        {
            return SplitSeparator.Count > 0 ? SplitSeparator.Cast<string>().ToArray() : new[] { defaultSeparator };
        }
        #endregion
    }
}