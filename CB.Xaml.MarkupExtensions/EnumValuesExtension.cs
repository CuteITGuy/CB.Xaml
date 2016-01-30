using System;
using System.Windows.Markup;


namespace CB.Xaml.MarkupExtensions
{
    public class EnumValuesExtension : MarkupExtension
    {
        #region Fields
        private readonly Type enumType;
        #endregion


        #region Constructors & Destructors
        public EnumValuesExtension(Type enumType)
        {
            this.enumType = enumType;
        }
        #endregion


        #region Methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (enumType.IsEnum)
            {
                return Enum.GetValues(enumType);
            }
            throw new InvalidOperationException("Provided type is not a valid enum type.");
        }
        #endregion
    }
}