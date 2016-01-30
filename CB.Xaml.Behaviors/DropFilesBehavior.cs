using System.Collections.Generic;
using System.Windows.Controls;
using CB.Xaml.Behaviors.Impl;


namespace CB.Xaml.Behaviors
{
    public class DropFilesBehavior : DropFilesBehaviorImpl<ListBox>
    {
        protected override void OnFilesDrop(IEnumerable<string> filePaths)
        {
            AssociatedObject.ItemsSource = filePaths;
        }
    }
}