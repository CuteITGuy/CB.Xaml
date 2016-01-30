namespace CB.Xaml.Behaviors.Impl
{
    public class Word
    {
        #region Properties
        public int StartIndex { get; set; }

        public string Content { get; set; }
        #endregion


        #region Constructors
        public Word()
        {
            
        }

        public Word(int startIndex, string content)
        {
            StartIndex = startIndex;
            Content = content;
        }
        #endregion
    }
}