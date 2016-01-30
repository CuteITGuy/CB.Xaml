using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;


namespace CB.Xaml.Commands
{
    public class MultiKeyGesture : KeyGesture
    {
        #region Fields & Properties
        private static readonly TimeSpan maxDelayBetweenPresses = TimeSpan.FromMilliseconds(500);
        private DateTime lastPress;
        private int currentKeyIndex;

        private Key[] keys;

        public Key[] Keys
        {
            get { return keys; }
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentException("Keys");
                }
                if (value.Any(k => !IsValid(Key.K, Modifiers)))
                {
                    throw new ArgumentException("Keys");
                }
                keys = value;
            }
        }
        #endregion


        #region Constructors
        public MultiKeyGesture(params Key[] keys)
            : this(ModifierKeys.None, keys)
        {
        }

        public MultiKeyGesture(ModifierKeys modifiers, params Key[] keys)
            : this(modifiers, string.Empty, keys)
        {
        }

        public MultiKeyGesture(ModifierKeys modifiers, string displayString, params Key[] keys)
            : base(Key.None, modifiers, displayString)
        {
            Keys = keys;
        }
        #endregion


        #region Methods
        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            var args = inputEventArgs as KeyEventArgs;

            if ((args == null) || !IsDefinedKey(args.Key))
            {
                return false;
            }
            Debug.WriteLine("Modifiers: " + Keyboard.Modifiers);

            if (IsTooLongFromLastKey() || IsWrongModifiers() || IsWrongKey(args.Key))
            {
                currentKeyIndex = 0;
                return false;
            }

            ++currentKeyIndex;

            if (IsMatchingContinued())
            {
                lastPress = DateTime.Now;
                inputEventArgs.Handled = true;
                return false;
            }

            currentKeyIndex = 0;
            Debug.WriteLine("Match completed!");
            return true;
        }
        #endregion


        #region Implementation
        private static bool IsDefinedKey(Key key)
        {
            return ((key >= Key.None) && (key <= Key.OemClear));
        }

        private bool IsMatchingContinued()
        {
            var result = currentKeyIndex != keys.Length;
            Debug.WriteLineIf(result, "IsMatchingContinued");
            return result;
        }

        private bool IsTooLongFromLastKey()
        {
            var pressInterval = DateTime.Now - lastPress;
            var result = currentKeyIndex != 0 && pressInterval > maxDelayBetweenPresses;
            Debug.WriteLineIf(result, "IsTooLongFromLastKey: " + pressInterval.TotalMilliseconds);
            return result;
        }

        private static bool IsValid(Key key, ModifierKeys modifiers)
        {
            if (((key >= Key.F1) && (key <= Key.F24)) || ((key >= Key.NumPad0) && (key <= Key.Divide)))
            {
                return true;
            }

            if ((modifiers & (ModifierKeys.Windows | ModifierKeys.Control | ModifierKeys.Alt)) == ModifierKeys.None)
            {
                return ((key < Key.D0) || (key > Key.D9)) && ((key < Key.A) || (key > Key.Z));
            }

            switch (key)
            {
                case Key.LWin:
                case Key.RWin:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftAlt:
                case Key.RightAlt:
                    return false;
            }
            return true;
        }

        private bool IsWrongKey(Key key)
        {
            var result = keys[currentKeyIndex] != key;
            Debug.WriteLineIf(result, "IsWrongKey: " + key);
            return result;
        }

        private bool IsWrongModifiers()
        {
            var result = currentKeyIndex == 0 && Modifiers != Keyboard.Modifiers;
            Debug.WriteLineIf(result, "IsWrongModifiers: " + Keyboard.Modifiers);
            return result;
        }
        #endregion
    }
}