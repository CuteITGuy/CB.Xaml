using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using CB.Xaml.MarkupExtensions.Impl;


namespace CB.Xaml.MarkupExtensions
{
    public class FolderContentExtension: MarkupExtension
    {
        #region  Constructors & Destructors
        public FolderContentExtension(string folderPath)
        {
            FolderPath = folderPath;
        }

        public FolderContentExtension() { }
        #endregion


        #region  Properties & Indexers
        public EntryType EntryType { get; set; } = EntryType.FileAndFolder;

        public string Filter { get; set; }

        public string FolderPath { get; set; }

        public SearchOption SearchOption { get; set; } = SearchOption.TopDirectoryOnly;
        #endregion


        #region Override
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!string.IsNullOrEmpty(FolderPath))
            {
                if (string.IsNullOrEmpty(Filter))
                {
                    switch (EntryType)
                    {
                        case EntryType.File:
                            return Directory.GetFiles(FolderPath, "*", SearchOption);
                        case EntryType.Folder:
                            return Directory.GetDirectories(FolderPath, "*", SearchOption);
                        case EntryType.FileAndFolder:
                            return Directory.GetFileSystemEntries(FolderPath, "*", SearchOption);
                        default:
                            return DependencyProperty.UnsetValue;
                    }
                }

                var patterns = Filter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).AsParallel();
                switch (EntryType)
                {
                    case EntryType.File:
                        return
                            patterns.SelectMany(pattern => Directory.GetFiles(FolderPath, pattern, SearchOption))
                                    .ToArray();

                    case EntryType.Folder:
                        return
                            patterns.SelectMany(pattern => Directory.GetDirectories(FolderPath, pattern, SearchOption))
                                    .ToArray();

                    case EntryType.FileAndFolder:
                        return
                            patterns.SelectMany(
                                pattern => Directory.GetFileSystemEntries(FolderPath, pattern, SearchOption)).ToArray();

                    default:
                        return DependencyProperty.UnsetValue;
                }
            }
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
}