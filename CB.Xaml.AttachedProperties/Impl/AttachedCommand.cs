using System;
using System.Windows;
using System.Windows.Input;
using CB.Dynamic.CompilerServices;


namespace CB.Xaml.AttachedProperties.Impl
{
    public class AttachedCommand: DependencyObject
    {
        #region Fields
        private EventHandler _canExecuteChanged;
        private Delegate _handler;
        #endregion


        #region  Properties & Indexers
        public bool UseCanExecute { get; set; }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(AttachedCommand),
            new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(AttachedCommand),
            new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
            nameof(EventName), typeof(string), typeof(AttachedCommand),
            new PropertyMetadata(default(string)));

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }
        #endregion


        #region Methods
        public void Attach(FrameworkElement element)
        {
            _handler = EventHandlerAttacher.Attach(element, EventName, () => Command.Execute(CommandParameter));
            if (UseCanExecute)
            {
                Command.CanExecuteChanged +=
                    _canExecuteChanged ??
                    (_canExecuteChanged = delegate { element.IsEnabled = Command.CanExecute(CommandParameter); });
            }
        }

        public void Detach(FrameworkElement element)
        {
            EventHandlerAttacher.Detach(element, EventName, _handler);
            if (UseCanExecute) Command.CanExecuteChanged -= _canExecuteChanged;
        }
        #endregion
    }
}