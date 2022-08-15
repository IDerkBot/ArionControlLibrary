using System;
using System.Windows;
using System.Windows.Controls;

namespace ArionControlLibrary
{
    public static class Extensions
    {
        public static void ChangeContentAsync(this ContentControl control, object content)
        {
            control.Dispatcher.BeginInvoke(new Action(() => control.Content = content));
        }
        public static void ChangeEnable(this UIElement control, bool enable)
        {
            control.Dispatcher.BeginInvoke(new Action(() => control.IsEnabled = enable));
        }

        public static void ChangeVisible(this UIElement control, bool visible)
        {
            control.Dispatcher.BeginInvoke(new Action(() => control.Visibility = visible ? Visibility.Visible : Visibility.Collapsed));
        }
    }
}