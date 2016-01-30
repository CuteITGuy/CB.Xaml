using System;
using System.Globalization;
using System.Windows.Data;
using Humanizer;


namespace CB.Xaml.ValueConverters
{
    public abstract class HumanizedStringConverterBase: IValueConverter
    {
        #region Fields
        protected LetterCasing _letterCasing = LetterCasing.Title;
        #endregion


        #region Abstract
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        #endregion


        #region  Properties & Indexers
        public HumanizingEncoding HumanizingEncoding { get; set; } = HumanizingEncoding.Pascalize;

        public LetterCasing LetterCasing
        {
            get { return _letterCasing; }
            set { _letterCasing = value; }
        }

        public Type SourceType { get; set; }
        #endregion


        #region Implementation
        protected virtual string DehumanizeString(string stringValue)
        {
            var underScoreString = stringValue.Replace(' ', '_');
            string dehumanizedString;

            switch (HumanizingEncoding)
            {
                case HumanizingEncoding.Camelize:
                    dehumanizedString = underScoreString.Camelize();
                    break;

                case HumanizingEncoding.Underscore:
                    dehumanizedString = underScoreString;
                    break;

                default:
                    dehumanizedString = underScoreString.Pascalize();
                    break;
            }
            return dehumanizedString;
        }
        #endregion
    }
}