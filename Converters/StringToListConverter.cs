using Avalonia.Data.Converters;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Pathfinder.Converters
{
    public class StringToListConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<string> list)
                return string.Join(Environment.NewLine, list); // Join with newlines
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string text)
                return new ObservableCollection<string>(
                    text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()));
            return new ObservableCollection<string>();
        }
    }
}