using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.Linq;

namespace Pathfinder.Converters
{
    public class LineNumberConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string text && !string.IsNullOrEmpty(text))
            {
                var lineCount = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length;
                return string.Join(Environment.NewLine, Enumerable.Range(1, lineCount).Select(i => i.ToString()));
            }
            return "1"; // Default to "1" if empty
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Not needed for one-way binding
        }
    }
}