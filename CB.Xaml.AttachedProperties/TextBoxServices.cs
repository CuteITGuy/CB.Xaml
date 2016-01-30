using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CB.Xaml.Commands;


namespace CB.Xaml.AttachedProperties
{
    public static class TextBoxServices
    {
        #region  Fields
        private static readonly List<DependencyObject> clkObjects = new List<DependencyObject>();
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty AllowFileDropProperty = DependencyProperty.RegisterAttached(
            "AllowFileDrop", typeof(bool), typeof(TextBoxServices),
            new PropertyMetadata(false, OnAllowFileDropChanged));

        private static void OnAllowFileDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null || !(bool)e.NewValue) return;
            
                textBox.AllowDrop = true;
                textBox.PreviewDragOver += textBox_PreviewDragOver;
                textBox.DragEnter += textBox_DragEnter;
                textBox.Drop += textBox_Drop;
        }

        [Category("TextBoxServices")]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetAllowFileDrop(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowFileDropProperty);
        }

        [Category("TextBoxServices")]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetAllowFileDrop(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowFileDropProperty, value);
        }

        public static readonly DependencyProperty ClickedObjectProperty = DependencyProperty.RegisterAttached(
            "ClickedObject", typeof(DependencyObject), typeof(TextBoxServices),
            new PropertyMetadata(null, null, CoerceClickedObject));

        [Category("TextBoxServices")]
        [AttachedPropertyBrowsableForType(typeof(IInputElement))]
        public static DependencyObject GetClickedObject(DependencyObject obj)
        {
            return (DependencyObject)obj.GetValue(ClickedObjectProperty);
        }

        [Category("TextBoxServices")]
        [AttachedPropertyBrowsableForType(typeof(IInputElement))]
        public static void SetClickedObject(DependencyObject obj, DependencyObject value)
        {
            obj.SetValue(ClickedObjectProperty, value);
        }

        private static object CoerceClickedObject(DependencyObject d, object baseValue)
        {
            if (!clkObjects.Contains(d) && d is IInputElement)
            {
                ((IInputElement)d).KeyDown += inputElement_KeyDown;
                clkObjects.Add(d);
            }
            return baseValue;
        }

        public static readonly DependencyProperty TextCommandsProperty = DependencyProperty.RegisterAttached(
            "TextCommandFlags", typeof(TextCommandFlags), typeof(TextBoxServices),
            new PropertyMetadata(default(TextCommandFlags), OnTextCommandsChanged));

        private static void OnTextCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null)
            {
                return;
            }

            AddOrRemoveCommandBindings(textBox, (TextCommandFlags)e.NewValue);
        }

        [Category("TextBoxServices")]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static TextCommandFlags GetTextCommands(DependencyObject obj)
        {
            return (TextCommandFlags)obj.GetValue(TextCommandsProperty);
        }

        [Category("TextBoxServices")]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetTextCommands(DependencyObject obj, TextCommandFlags value)
        {
            obj.SetValue(TextCommandsProperty, value);
        }
        #endregion


        #region Event Handlers
        private static void inputElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var clickedObj = GetClickedObject((DependencyObject)e.Source);

                // Assure clickObj is not null and is a UIElement object or a ContentElement object which contains a Click event
                if (clickedObj is UIElement || clickedObj is ContentElement)
                {
                    ((dynamic)clickedObj).RaiseEvent(new RoutedEventArgs(GetClickEvent(clickedObj)));
                }
            }
        }

        private static void textBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
        }

        private static void textBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var filesDrop = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (filesDrop != null && filesDrop.Length > 0)
                {
                    var textBox = sender as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = filesDrop[0];
                    }
                }
            }
        }

        private static void textBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
        #endregion


        #region Implementation
        private static void AddOrRemoveCommandBindings(TextBox textBox, TextCommandFlags textCommandFlags)
        {
            var commandBindingCollection = textBox.CommandBindings;
            var commandBindings = commandBindingCollection.Cast<CommandBinding>().ToArray();

            CheckCommandBinding(
                textBox, textCommandFlags, commandBindings, commandBindingCollection, TextCommands.Trim,
                TextCommandFlags.TrimAll, TextCommandType.TrimAll);
            CheckCommandBinding(
                textBox, textCommandFlags, commandBindings, commandBindingCollection,
                TextCommands.TrimEnd, TextCommandFlags.TrimEnd, TextCommandType.TrimEnd);
            CheckCommandBinding(
                textBox, textCommandFlags, commandBindings, commandBindingCollection,
                TextCommands.TrimStart, TextCommandFlags.TrimStart, TextCommandType.TrimStart);
        }

        private static void CheckCommandBinding(
            TextBox textBox, TextCommandFlags textCommandFlags,
            IEnumerable<CommandBinding> commandBindings, CommandBindingCollection commandBindingCollection,
            RoutedUICommand command, TextCommandFlags commandFlag, TextCommandType commandType)
        {
            var commandBinding = commandBindings.FirstOrDefault(cb => cb.Command == command);
            if (textCommandFlags.HasFlag(commandFlag) && commandBinding == null)
            {
                commandBindingCollection.Add(TextCommandBindings.CreateCommandBinding(textBox, commandType));
            }
            else if (!textCommandFlags.HasFlag(commandFlag) && commandBinding != null)
            {
                commandBindingCollection.Remove(commandBinding);
            }
        }

        private static RoutedEvent GetClickEvent(object element)
        {
            RoutedEvent res = null;
            for (var type = element.GetType();
                 type != null && type != typeof(object) && res == null;
                 type = type.BaseType)
            {
                var routedEvents = EventManager.GetRoutedEventsForOwner(type);
                if (routedEvents != null)
                {
                    res = routedEvents.FirstOrDefault(e => e.Name == "Click");
                }
            }
            return res;
        }
        #endregion
    }
}