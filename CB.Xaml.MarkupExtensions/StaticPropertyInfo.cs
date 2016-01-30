namespace CB.Xaml.MarkupExtensions
{
    public class StaticPropertyInfo
    {
        #region Properties & Indexers
        public string Name { get; set; }

        public object Value { get; set; }
        #endregion


        #region Constructors & Destructors
        public StaticPropertyInfo(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public StaticPropertyInfo()
        {
        }
        #endregion


        #region Methods
        public override string ToString()
        {
            return Name ?? base.ToString();
        }
        #endregion
    }
}