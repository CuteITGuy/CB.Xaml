using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using CursorConverter = System.Windows.Input.CursorConverter;
using Image = System.Windows.Controls.Image;


namespace CB.Xaml.AttachedProperties
{
    public static class CursorServices
    {
        #region Attached Properites
        [Category("CursorServices")]
        [AttachedPropertyBrowsableForType(typeof (Image))]
        public static string GetSource(DependencyObject obj)
        {
            return (string) obj.GetValue(SourceProperty);
        }

        [Category("CursorServices")]
        [AttachedPropertyBrowsableForType(typeof (Image))]
        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source",
            typeof (string), typeof (CursorServices), new PropertyMetadata(null, OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var img = d as Image;
            if (img == null)
            {
                return;
            }

            var source = e.NewValue as string;
            if (source != null)
            {
                ViewCursor(img, source);
            }
        }

        private static void ViewCursor(Image img, string source)
        {
            var cursor = GetCursor(source);
            if (cursor == null)
            {
                return;
            }

            using (var bmp = new Bitmap(cursor.Size.Width, cursor.Size.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    var drawingRect = new Rectangle(0, 0, cursor.Size.Width, cursor.Size.Height);
                    g.FillRectangle(Brushes.White, drawingRect);
                    cursor.Draw(g, drawingRect);
                }
                var ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                img.Source = bi;
            }
        }

        private static Cursor GetCursor(string source)
        {
            var curConverter = new CursorConverter();
            var cursor = curConverter.ConvertFromString(source) as System.Windows.Input.Cursor;
            if (cursor == null)
            {
                return null;
            }

            var safeHandleType = typeof (SafeHandle);
            var handleField = typeof (System.Windows.Input.Cursor).GetFields(BindingFlags.Instance |
                                                                             BindingFlags.Public |
                                                                             BindingFlags.NonPublic)
                .FirstOrDefault(f => f.FieldType == safeHandleType || f.FieldType.IsSubclassOf(safeHandleType));

            if (handleField == null)
            {
                return null;
            }

            var curHandle = handleField.GetValue(cursor) as SafeHandle;
            if (curHandle == null)
            {
                return null;
            }

            var hndl = curHandle.DangerousGetHandle();
            return hndl != IntPtr.Zero ? new Cursor(hndl) : null;
        }

        /*private static System.Windows.Forms.Cursor GetCursor(string source)
        {
            System.Windows.Forms.Cursor cursor = null;

            if (!string.IsNullOrEmpty(source))
            {
                source = source.Trim();

                // Search in file system
                if (File.Exists(source))
                {
                    var curHndl = GC.WinAPI.Cursor.LoadCursor(source);
                    if (curHndl != IntPtr.Zero)
                    {
                        cursor = new System.Windows.Forms.Cursor(curHndl);
                    }
                }
                else
                {
                    // Search in system cursors
                    CursorType curType;
                    if (Enum.TryParse(source, out curType))
                    {
                        cursor = GetSystemCursor(curType);
                    }
                    else
                    {
                        // Search in assembly resource
                        Assembly asm = Assembly.GetEntryAssembly();
                        var resNames = asm.GetManifestResourceNames().Select(s => s.Replace(".resources", ""));
                        / *var resName = asm.GetName().Name + ".g";
                        var resMng = new ResourceManager(resName, asm);
                        using (ResourceSet resSet = resMng.GetResourceSet(CultureInfo.CurrentCulture, true, true))
                        {
                            var curStream = resSet.GetObject(source, true) as UnmanagedMemoryStream;
                            if (curStream != null)
                            {
                                var curHndl = GC.WinAPI.Cursor.LoadCursor(curStream);
                                if (curHndl != IntPtr.Zero)
                                {
                                    cursor = new System.Windows.Forms.Cursor(curHndl);
                                }
                            }
                        }* /
                        foreach (var resName in resNames)
                        {
                            var resMng = new ResourceManager(resName, asm);
                            using (ResourceSet resSet = resMng.GetResourceSet(CultureInfo.CurrentCulture, true, true))
                            {
                                var curStream = resSet.GetObject(source, true) as UnmanagedMemoryStream;
                                if (curStream != null)
                                {
                                    var curHndl = GC.WinAPI.Cursor.LoadCursor(curStream);
                                    if (curHndl != IntPtr.Zero)
                                    {
                                        cursor = new System.Windows.Forms.Cursor(curHndl);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return cursor;
        }*/

        /*private static System.Windows.Forms.Cursor GetSystemCursor(CursorType curType)
        {
            switch (curType)
            {
                case CursorType.AppStarting: return System.Windows.Forms.Cursors.AppStarting;
                case CursorType.Arrow: return System.Windows.Forms.Cursors.Arrow;
                case CursorType.ArrowCD: 
                    break;
                case CursorType.Cross:
                    break;
                case CursorType.Hand:
                    break;
                case CursorType.Help:
                    break;
                case CursorType.IBeam:
                    break;
                case CursorType.No:
                    break;
                case CursorType.None:
                    break;
                case CursorType.Pen:
                    break;
                case CursorType.ScrollAll:
                    break;
                case CursorType.ScrollE:
                    break;
                case CursorType.ScrollN:
                    break;
                case CursorType.ScrollNE:
                    break;
                case CursorType.ScrollNS:
                    break;
                case CursorType.ScrollNW:
                    break;
                case CursorType.ScrollS:
                    break;
                case CursorType.ScrollSE:
                    break;
                case CursorType.ScrollSW:
                    break;
                case CursorType.ScrollW:
                    break;
                case CursorType.ScrollWE:
                    break;
                case CursorType.SizeAll:
                    break;
                case CursorType.SizeNESW:
                    break;
                case CursorType.SizeNS:
                    break;
                case CursorType.SizeNWSE:
                    break;
                case CursorType.SizeWE:
                    break;
                case CursorType.UpArrow:
                    break;
                case CursorType.Wait:
                    break;
                default:
                    break;
            }
        }*/
        #endregion
    }
}