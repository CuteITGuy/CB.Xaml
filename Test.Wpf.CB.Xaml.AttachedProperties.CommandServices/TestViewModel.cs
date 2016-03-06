using System.Windows.Input;
using CB.Model.Common;


namespace Test.Wpf.CB.Xaml.AttachedProperties.CommandServices
{
    public class TestViewModel: ViewModelBase
    {
        #region Fields
        private ICommand _addCommand;
        private int _max = 100;
        private int _min = -100;
        private ICommand _subtractCommand;
        private int _value;
        #endregion


        #region  Properties & Indexers
        public ICommand AddCommand => GetCommand(ref _addCommand, par => Add((int)par), par => CanAdd((int)par));

        public int Max
        {
            get { return _max; }
            set { SetProperty(ref _max, value); }
        }

        public int Min
        {
            get { return _min; }
            set { SetProperty(ref _min, value); }
        }

        public ICommand SubtractCommand
            => GetCommand(ref _subtractCommand, par => Subtract((int)par), par => CanSubtract((int)par));

        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
        #endregion


        #region Methods
        public void Add(int add)
        {
            if (CanAdd(add)) Value += add;
        }

        public bool CanAdd(int add) => Value + add < Max;

        public bool CanSubtract(int subtract) => Value - subtract > Min;

        public void Subtract(int subtract)
        {
            if(CanSubtract(subtract)) Value -= subtract;
        }
        #endregion
    }
}