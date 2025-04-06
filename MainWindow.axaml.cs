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
            var parts = query.Split(new[] { " OR ", ",\n" }, StringSplitOptions.RemoveEmptyEntries)
                            .Where(p => !string.IsNullOrWhiteSpace(p))
                            .ToList();

            bool highlight = true; // Start with highlighting for the first part
            foreach (var part in parts)
            {
                var highlightBrush = GetHighlightBrush(viewModel.CurrentTheme!);
                var run = new Run(part.Trim())
                {
                    Background = highlight ? highlightBrush : viewModel.CurrentTheme!.TextBoxBackground,
                    Foreground = viewModel.CurrentTheme!.TextForeground
                };
                textBlock.Inlines.Add(run);
                if (part != parts.Last()) textBlock.Inlines.Add(new Run(" OR ") { Foreground = viewModel.CurrentTheme!.TextForeground });
                highlight = !highlight; // Toggle for binary effect
            }
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