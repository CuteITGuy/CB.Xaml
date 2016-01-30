using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;


namespace CB.Xaml.MarkupExtensions
{
    public class InstancePropertyNamesExtension : MarkupExtension
    {
        #region Fields
        private readonly Type type;
        #endregion


        #region Constructors & Destructors
        public InstancePropertyNamesExtension(Type type)
        {
            this.type = type;
        }
        #endregion


        #region Methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name).ToArray();
        }
        #endregion
    }
}