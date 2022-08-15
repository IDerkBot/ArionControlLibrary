using Accord.Video.FFMPEG;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArionControlLibrary.Utilities
{
    public class VideoCamera
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            try
            {
                var handle = bmp.GetHbitmap();
                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally { DeleteObject(handle); }
            }
            catch (NullReferenceException)
            {
                var handle = new Bitmap("").GetHbitmap();
                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally { DeleteObject(handle); }
            }
        }

        public void UpdateImage(VideoFileReader videoReader, System.Windows.Controls.Image image)
        {
            new Thread(() =>
            {
                while (videoReader.IsOpen)
                {
                    try
                    {
                        Bitmap frame = videoReader.ReadVideoFrame();
                        image.Dispatcher.BeginInvoke(new Action(() => image.Source = ImageSourceFromBitmap(frame)));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }).Start();
        }
    }
}
