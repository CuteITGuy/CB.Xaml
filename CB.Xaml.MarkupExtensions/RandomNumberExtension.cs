using System;
using System.Windows.Markup;


namespace CB.Xaml.MarkupExtensions
{
    public class RandomNumberExtension : MarkupExtension
    {
        #region Fields
        private static readonly Random random = new Random(DateTime.Now.Millisecond);
        #endregion


        #region Properties & Indexers
        public bool IntegralTarget { get; set; }

        public double Max { get; set; }

        public double Min { get; set; }
        #endregion


        #region Constructors & Destructors
        public RandomNumberExtension() : this(10.0)
        {
        }

        public RandomNumberExtension(double max, double min = 0.0, bool integralTarget = true)
        {
            Max = max;
            Min = min;
            IntegralTarget = integralTarget;
        }
        #endregion


        #region Methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IntegralTarget)
            {
                return random.Next((int) Min, (int) Max + 1);
            }
            return random.NextDouble()*(Max - Min) + Min;
        }
        #endregion
    }
}