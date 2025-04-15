using Avalonia.Data.Converters;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Pathfinder.Converters
{
    public class CollectionToEnabledConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<string> collection)
            {
                return collection.Count == 0;
            }
            return true;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}