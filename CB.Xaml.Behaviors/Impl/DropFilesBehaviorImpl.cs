using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using CB.IO.Common;


namespace CB.Xaml.Behaviors.Impl
{
    public abstract class DropFilesBehaviorImpl<TElement>: Behavior<TElement> where TElement: UIElement
    {
        #region Abstract
        protected abstract void OnFilesDrop(IEnumerable<string> filePaths);
        #endregion


        #region  Properties & Indexers
        public PathType PathType { get; set; } = PathType.FullPath;
        #endregion


        #region Override
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
        #endregion


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
            OnFilesDrop(GetFilePaths(filesDrop));
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

        private IEnumerable<string> GetFilePaths(string[] paths)
        {
            if (paths == null) throw new ArgumentNullException(nameof(paths));

            switch (PathType)
            {
                case PathType.FullPath:
                    return paths;

                case PathType.FullPathWithouExtension:
                    return paths.Select(GetPathWithoutExtension);

                case PathType.FileName:
                    return paths.Select(Path.GetFileName);

                case PathType.FileNameWithoutExtension:
                    return paths.Select(Path.GetFileNameWithoutExtension);

                case PathType.Extension:
                    return paths.Select(Path.GetExtension);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string GetPathWithoutExtension(string filePath)
            => Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));

        private void RemoveHandlers()
        {
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            AssociatedObject.Drop -= AssociatedObject_Drop;
            AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
        }
        #endregion
    }
}