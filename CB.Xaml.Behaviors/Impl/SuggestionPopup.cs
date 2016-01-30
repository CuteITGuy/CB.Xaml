using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CB.Wpf.Controls;


namespace CB.Xaml.Behaviors.Impl
{
    public class SuggestionPopup : ListPopup
    {
        #region Fields
        private const double DEFAULT_OPACITY = 0.92;
        private const double MIN_OPACITY = 0.275;
        private const double OPACITY_DECREASE = 0.06;
        #endregion


        #region Constructors & Destructors
        static SuggestionPopup()
        {
            PlacementTargetProperty.OverrideMetadata(typeof (SuggestionPopup),
                new FrameworkPropertyMetadata(OnPlacementTargetChanged));
        }

        public SuggestionPopup()
        {
            AllowsTransparency = true;
            ResetPopupOpacity();
        }
        #endregion


        #region Overridden
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            SetPopupFullOpacity();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            ResetPopupOpacity();
        }

        protected override void OnSettingListBoxItemStyle(ResourceDictionary resourceDictionary,
            IList<SetterBase> setters, IList<TriggerBase> triggers)
        {
            base.OnSettingListBoxItemStyle(resourceDictionary, setters, triggers);

            var previewMouseMoveEventSetter = new EventSetter(PreviewMouseMoveEvent,
                new MouseEventHandler(ListBoxItems_PreviewMouseMove));
            setters.Add(previewMouseMoveEventSetter);
        }
        #endregion


        #region Event Handlers
        private void ListBoxItems_PreviewMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var item = sender as ListBoxItem;
            if (item != null)
            {
                _listBox.SelectedItem = item.Content;
            }
        }

        private void Target_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (IsOpen)
            {
                TargetPreviewKeyDownWhenPopupOpen(e);
            }
        }

        private void Target_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (IsOpen)
            {
                TargetPreviewKeyUpWhenPopupOpen(e);
            }
        }
        #endregion


        #region Implementation
        private void AddTargetHandlers(UIElement newTarget)
        {
            newTarget.PreviewKeyDown += Target_PreviewKeyDown;
            newTarget.PreviewKeyUp += Target_PreviewKeyUp;
        }

        private void DecreasePopupOpacity()
        {
            var opacity = Opacity - OPACITY_DECREASE;
            Opacity = opacity > MIN_OPACITY ? opacity : MIN_OPACITY;
        }

        private static void OnPlacementTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisElement = d as SuggestionPopup;
            if (thisElement == null)
            {
                return;
            }

            var newTarget = e.NewValue as UIElement;
            if (newTarget != null)
            {
                thisElement.AddTargetHandlers(newTarget);
            }

            var oldTarget = e.OldValue as UIElement;
            if (oldTarget != null)
            {
                thisElement.RemoveTargetHandlers(oldTarget);
            }
        }

        private void RemoveTargetHandlers(IInputElement oldTarget)
        {
            oldTarget.PreviewKeyDown -= Target_PreviewKeyDown;
            oldTarget.PreviewKeyUp -= Target_PreviewKeyUp;
        }

        private void ResetPopupOpacity()
        {
            Opacity = DEFAULT_OPACITY;
        }

        private void SetPopupFullOpacity()
        {
            Opacity = 1.0;
        }

        private void TargetPreviewKeyDownWhenPopupOpen(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    if (e.IsRepeat)
                    {
                        DecreasePopupOpacity();
                    }
                    break;
            }
        }

        private void TargetPreviewKeyUpWhenPopupOpen(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    ResetPopupOpacity();
                    break;
            }
        }
        #endregion
    }
}