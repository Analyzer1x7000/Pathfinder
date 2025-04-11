using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Pathfinder.ViewModels;
using ReactiveUI;
using System;
using System.Linq;

namespace Pathfinder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
            AttachQueryUpdateHandlers();
            // Force initial update for all outputs
            if (DataContext is MainViewModel viewModel)
            {
                UpdateOutputTextBlock(this.FindControl<TextBlock>("SentinelOneOutput"), viewModel.SentinelOneQuery, viewModel);
                UpdateOutputTextBlock(this.FindControl<TextBlock>("CrowdStrikeOutput"), viewModel.CrowdStrikeQuery, viewModel);
                UpdateOutputTextBlock(this.FindControl<TextBlock>("DefenderOutput"), viewModel.DefenderQuery, viewModel);
                UpdateOutputTextBlock(this.FindControl<TextBlock>("CBResponseOutput"), viewModel.CBResponseQuery, viewModel);
                UpdateOutputTextBlock(this.FindControl<TextBlock>("CBCloudOutput"), viewModel.CBCloudQuery, viewModel);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AttachQueryUpdateHandlers()
        {
            if (DataContext is not MainViewModel viewModel)
            {
                throw new InvalidOperationException("DataContext must be MainViewModel");
            }

            viewModel.WhenAnyValue(vm => vm.SentinelOneQuery).Subscribe(query => Dispatcher.UIThread.InvokeAsync(() => UpdateOutputTextBlock(this.FindControl<TextBlock>("SentinelOneOutput"), query, viewModel)));
            viewModel.WhenAnyValue(vm => vm.CrowdStrikeQuery).Subscribe(query => Dispatcher.UIThread.InvokeAsync(() => UpdateOutputTextBlock(this.FindControl<TextBlock>("CrowdStrikeOutput"), query, viewModel)));
            viewModel.WhenAnyValue(vm => vm.DefenderQuery).Subscribe(query => Dispatcher.UIThread.InvokeAsync(() => UpdateOutputTextBlock(this.FindControl<TextBlock>("DefenderOutput"), query, viewModel)));
            viewModel.WhenAnyValue(vm => vm.CBResponseQuery).Subscribe(query => Dispatcher.UIThread.InvokeAsync(() => UpdateOutputTextBlock(this.FindControl<TextBlock>("CBResponseOutput"), query, viewModel)));
            viewModel.WhenAnyValue(vm => vm.CBCloudQuery).Subscribe(query => Dispatcher.UIThread.InvokeAsync(() => UpdateOutputTextBlock(this.FindControl<TextBlock>("CBCloudOutput"), query, viewModel)));
            // Subscribe to theme changes to update all outputs
            viewModel.WhenAnyValue(vm => vm.CurrentTheme).Subscribe(_ => UpdateAllOutputs(viewModel));
        }

        private void UpdateAllOutputs(MainViewModel viewModel)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                UpdateOutputTextBlock(this.FindControl<TextBlock>("SentinelOneOutput"), viewModel.SentinelOneQuery, viewModel);
                UpdateOutputTextBlock(this.FindControl<TextBlock>("CrowdStrikeOutput"), viewModel.CrowdStrikeQuery, viewModel);
                UpdateOutputTextBlock(this.FindControl<TextBlock>("DefenderOutput"), viewModel.DefenderQuery, viewModel);
                UpdateOutputTextBlock(this.FindControl<TextBlock>("CBResponseOutput"), viewModel.CBResponseQuery, viewModel);
                UpdateOutputTextBlock(this.FindControl<TextBlock>("CBCloudOutput"), viewModel.CBCloudQuery, viewModel);
            });
        }

        private void UpdateOutputTextBlock(TextBlock? textBlock, string? query, MainViewModel viewModel)
        {
            if (textBlock == null || string.IsNullOrEmpty(query)) return;

            textBlock.Inlines!.Clear();

            // Special case for "No IOCs entered"
            if (query == "No IOCs entered")
            {
                textBlock.Inlines.Add(new Run(query)
                {
                    Background = GetHighlightBrush(viewModel.CurrentTheme!),
                    Foreground = viewModel.CurrentTheme!.TextForeground
                });
                return;
            }

            // Determine if query has userHostParts and IOC parts
            string userHostPart = "";
            string iocPart = "";
            bool hasUserHost = query.Contains(" AND (") && !query.StartsWith("("); // Indicates userHostParts before IOCs

            if (hasUserHost && textBlock.Name != "DefenderOutput") // Defender uses different structure
            {
                int andIndex = query.IndexOf(" AND (");
                userHostPart = query.Substring(0, andIndex).Trim();
                iocPart = query.Substring(andIndex + 5).Trim(); // Skip " AND "
                if (iocPart.StartsWith("(") && iocPart.EndsWith(")")) iocPart = iocPart.Substring(1, iocPart.Length - 2); // Remove outer parentheses
            }
            else
            {
                iocPart = query; // No userHostParts, treat as IOCs only
            }

            // Split IOC parts based on EDR type
            var iocParts = textBlock.Name == "DefenderOutput" ? SplitDefenderQuery(iocPart) : SplitQuery(iocPart);

            // Add userHostPart if present (non-Defender EDRs)
            if (!string.IsNullOrEmpty(userHostPart) && textBlock.Name != "DefenderOutput")
            {
                textBlock.Inlines.Add(new Run(userHostPart.Trim())
                {
                    Background = GetHighlightBrush(viewModel.CurrentTheme!),
                    Foreground = viewModel.CurrentTheme!.TextForeground
                });
                textBlock.Inlines.Add(new Run(" AND ") { Foreground = viewModel.CurrentTheme!.TextForeground });
            }

            // Add IOC parts with alternating highlighting
            for (int i = 0; i < iocParts.Count; i++)
            {
                var highlightBrush = GetHighlightBrush(viewModel.CurrentTheme!);
                bool shouldHighlight = string.IsNullOrEmpty(userHostPart) ? (i % 2 == 0) : (i % 2 == 1);

                var run = new Run(iocParts[i].Trim())
                {
                    Background = shouldHighlight ? highlightBrush : viewModel.CurrentTheme!.TextBoxBackground,
                    Foreground = viewModel.CurrentTheme!.TextForeground
                };
                textBlock.Inlines.Add(run);

                if (i < iocParts.Count - 1)
                {
                    string separator = textBlock.Name == "DefenderOutput" && iocParts.Count > 1 ? ",\n" : " OR ";
                    textBlock.Inlines.Add(new Run(separator) { Foreground = viewModel.CurrentTheme!.TextForeground });
                }
            }

            // For Defender with multiple parts, prepend "union\n" if needed
            if (textBlock.Name == "DefenderOutput" && iocParts.Count > 1)
            {
                textBlock.Inlines.Insert(0, new Run("union\n") { Foreground = viewModel.CurrentTheme!.TextForeground });
            }
        }

        // No change needed to SplitQuery
        private System.Collections.Generic.List<string> SplitQuery(string query)
        {
            var parts = new System.Collections.Generic.List<string>();
            int start = 0;
            int parenCount = 0;

            for (int i = 0; i < query.Length; i++)
            {
                if (query[i] == '(') parenCount++;
                else if (query[i] == ')') parenCount--;

                if (parenCount == 0 && query.Substring(i).StartsWith(" OR "))
                {
                    parts.Add(query.Substring(start, i - start));
                    start = i + 4; // Skip " OR "
                    i = start - 1;
                }
            }

            if (start < query.Length)
            {
                parts.Add(query.Substring(start));
            }

            return parts;
        }

        // No change needed to SplitDefenderQuery
        private System.Collections.Generic.List<string> SplitDefenderQuery(string query)
        {
            if (!query.Contains("union\n")) return new System.Collections.Generic.List<string> { query };
            return query.Split(new[] { "union\n" }, StringSplitOptions.None)
                        .SelectMany(part => part.Split(new[] { ",\n" }, StringSplitOptions.None))
                        .Where(part => !string.IsNullOrWhiteSpace(part))
                        .ToList();
        }

        private ISolidColorBrush GetHighlightBrush(Theme theme)
        {
            return theme.Name switch
            {
                "Night Shift" => new SolidColorBrush(Color.Parse("#2A2A2A")), // Same as Onyx, contrasts with #222222
                "Matrix" => new SolidColorBrush(Color.Parse("#143C3C")), // Dark teal, contrasts with #0A0A0A
                "Strawberry Milkshake" => new SolidColorBrush(Color.Parse("#FFAAAA")), // Light pink, contrasts with #FFCCCC
                "Windows 95" => new SolidColorBrush(Color.Parse("#D3D3D3")), // Light gray, contrasts with #FFFFFF
                "Day Shift" => new SolidColorBrush(Color.Parse("#CCD1D9")), // Medium gray, contrasts with #E8ECEF and black text
                _ => new SolidColorBrush(Color.Parse("#2A2A2A")) // Fallback
            };
        }
    }
}