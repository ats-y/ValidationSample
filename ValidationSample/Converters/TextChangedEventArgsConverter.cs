using System;
using System.Globalization;
using Xamarin.Forms;

namespace ValidationSample.Converters
{
    /// <summary>
    /// TextChangedイベント引数をCommandパラメータに変換する。
    /// </summary>
    public class TextChangedEventArgsConverter : IValueConverter
    {
        public TextChangedEventArgsConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextChangedEventArgs args = value as TextChangedEventArgs;
            if (args == null)
            {
                throw new ArgumentException();
            }

            return args;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
