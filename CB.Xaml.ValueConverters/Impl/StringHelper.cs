using System;


namespace CB.Xaml.ValueConverters.Impl
{
    public class StringHelper
    {
        #region Fields
        private const string SPACE = " ";

        private const string TAB = "\t";
        #endregion


        #region  Properties & Indexers
        public static string NewLine { get; } = Environment.NewLine;

        public static string Space
        {
            get { return SPACE; }
        }

        public static string Tab
        {
            get { return TAB; }
        }
        #endregion
    }
}