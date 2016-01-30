using System;
using CB.Model.Common;


namespace Playground
{
    public class PlaygroundModel: ObservableObject
    {
        #region Fields
        private StringKind _kind;
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private string _stringValue;
        #endregion


        #region  Properties & Indexers
        public StringKind Kind
        {
            get { return _kind; }
            set { if (SetProperty(ref _kind, value)) SetStringValue(); }
        }

        public string StringValue
        {
            get { return _stringValue; }
            set { SetProperty(ref _stringValue, value); }
        }
        #endregion


        #region Implementation
        private void SetStringValue()
        {
            switch (Kind)
            {
                case StringKind.Null:
                    StringValue = null;
                    break;
                case StringKind.Empty:
                    StringValue = "";
                    break;
                case StringKind.Random:
                    StringValue = _random.Next().ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}