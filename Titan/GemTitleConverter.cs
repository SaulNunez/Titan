using System;
using Titan.Models;
using Windows.UI.Xaml.Data;

namespace Titan
{
    internal class GemTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is OnlineGemPage page)
            {
                return page.Title;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
