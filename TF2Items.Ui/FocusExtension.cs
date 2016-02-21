using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace TF2Items.Ui
{
    public static class FocusExtension
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }


        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }


        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
             "IsFocused", typeof(bool), typeof(FocusExtension),
             new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));


        private static void OnIsFocusedPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if ((bool)e.NewValue)
            {
                Debug.WriteLine("IsFocused");
                uie.Focus(); // Don't care about false values.
            }
        }
    }

    public static class SelectAllExtension
    {
        public static bool GetSelectAll(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectAllProperty);
        }


        public static void SetSelectAll(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectAllProperty, value);
        }


        public static readonly DependencyProperty SelectAllProperty =
            DependencyProperty.RegisterAttached(
             "SelectAll", typeof(bool), typeof(SelectAllExtension),
             new UIPropertyMetadata(false, OnSelectAllPropertyChanged));


        private static void OnSelectAllPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = d as TextBox;
            if (uie != null && (bool)e.NewValue)
            {
                Debug.WriteLine("SelectAll");

                Timer timer = new Timer(100)
                              {
                                  AutoReset = false,
                              };
                timer.Elapsed += (sender, args) =>
                                 {
                                     Application.Current.Dispatcher.Invoke(() => uie.SelectAll());
                                 };
                timer.Start();
            }
        }
    }

    public static class MouseCaptureExtension
    {
        public static bool GetCaptureMouse(DependencyObject obj)
        {
            return (bool)obj.GetValue(CaptureMouseProperty);
        }


        public static void SetCaptureMouse(DependencyObject obj, bool value)
        {
            obj.SetValue(CaptureMouseProperty, value);
        }


        public static readonly DependencyProperty CaptureMouseProperty =
            DependencyProperty.RegisterAttached(
             "CaptureMouse", typeof(bool), typeof(MouseCaptureExtension),
             new UIPropertyMetadata(false, OnSelectAllPropertyChanged));


        private static void OnSelectAllPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = d as TextBox;
            if (uie != null && (bool)e.NewValue)
            {
                Debug.WriteLine("CaptureMouse");
                uie.CaptureMouse(); // Don't care about false values.
            }
        }
    }
}