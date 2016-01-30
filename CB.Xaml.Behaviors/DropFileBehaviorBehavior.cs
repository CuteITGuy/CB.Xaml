using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using CB.Xaml.Behaviors.Impl;


namespace CB.Xaml.Behaviors
{
    /*public class DropFileBehaviorBehavior: Behavior<TextBox>
    {
        public PathType PathType { get; set; } = PathType.FullPath;

        #region Event Handlers
        private static void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            var filesDrop = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (filesDrop == null || filesDrop.Length == 0) return;
            AssociatedObject.Text = GetText(filesDrop[0]);
        }

        private string GetText(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            switch (PathType)
            {
                case PathType.FullPath:
                    return path;

                case PathType.FullPathWithouExtension:
                    return Path.Combine(Path.GetDirectoryName(path),Path.GetFileNameWithoutExtension(path));

                case PathType.FileName:
                    return Path.GetFileName(path);

                case PathType.FileNameWithoutExtension:
                    return Path.GetFileNameWithoutExtension(path);

                case PathType.Extension:
                    return Path.GetExtension(path);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
        #endregion


        #region Implementation
        private void AddHandlers()
        {
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
            AssociatedObject.Drop += AssociatedObject_Drop;
            AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            RemoveHandlers();
        }

        private void RemoveHandlers()
        {
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            AssociatedObject.Drop -= AssociatedObject_Drop;
            AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
        }
        #endregion
    }*/

    public class DropFileBehaviorBehavior: DropFilesBehaviorImpl<TextBox>
    {
        #region Override
        protected override void OnFilesDrop(IEnumerable<string> filePaths)
        {
            AssociatedObject.Text = filePaths.First();
        }
        #endregion
    }
}