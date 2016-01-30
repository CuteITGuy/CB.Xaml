using System;
using System.Windows.Markup;


namespace CB.Xaml.MarkupExtensions
{
    public class EnumNamesExtension : MarkupExtension
    {
        #region Fields
        private readonly Type enumType;
        #endregion


        #region Constructors & Destructors
        public EnumNamesExtension(Type enumType)
        {
            this.enumType = enumType;
        }
        #endregion


        #region Methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (enumType.IsEnum)
            {
                return Enum.GetNames(enumType);
            }
            throw new InvalidOperationException("Provided type is not a valid enum type.");
        }
        #endregion
    }
}