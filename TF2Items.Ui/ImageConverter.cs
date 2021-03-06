﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TF2Items.Ui
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return new BitmapImage();

                string str = (string)value;
                if (string.IsNullOrEmpty(str))
                {
                    return new BitmapImage();
                }
                return new BitmapImage(new Uri(str));
            }
            catch (Exception)
            {
                return new BitmapImage(new Uri("pack://application:,,,/TF2Items.Ui;component/assets/icons/error.png"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}