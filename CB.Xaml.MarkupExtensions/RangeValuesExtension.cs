using System;
using System.Linq;
using System.Windows.Markup;


namespace CB.Xaml.MarkupExtensions
{
    public class RangeValuesExtension : MarkupExtension
    {
        #region Fields
        private readonly int start;
        private readonly int count;
        #endregion


        #region Constructors & Destructors
        public RangeValuesExtension(int count, int start = 0)
        {
            this.count = count;
            this.start = start;
        }
        #endregion


        #region Methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enumerable.Range(start, count);
        }
        #endregion
    }
}