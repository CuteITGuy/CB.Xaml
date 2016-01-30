using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;


namespace CB.Xaml.MarkupExtensions
{
    public class StaticPropertiesExtension : MarkupExtension
    {
        #region Fields
        #endregion


        #region Properties & Indexers
        public Type DeclaringType { get; set; }

        public Type PropertyType { get; set; }
        #endregion


        #region Methods
        /// <summary>
        ///     Returns an array of StaticProperty objects each represents the name and the value of a public static property.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IEnumerable<PropertyInfo> props = DeclaringType.GetProperties(BindingFlags.Static | BindingFlags.Public);
            if (PropertyType != null)
            {
                props = props.Where(p => p.PropertyType == PropertyType || p.PropertyType.IsSubclassOf(PropertyType));
            }
            var result = props.Select(p => new StaticPropertyInfo(p.Name, p.GetValue(null))).ToArray();
            return result;
        }
        #endregion


        #region Constructors
        public StaticPropertiesExtension(Type type)
        {
            DeclaringType = type;
        }

        public StaticPropertiesExtension()
        {
        }
        #endregion
    }
}