using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;
using Avalonia.Styling;
using Avalonia.Platform;
using System;

namespace Pathfinder
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            Styles.Add(new FluentTheme());
            RequestedThemeVariant = ThemeVariant.Dark;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();
                mainWindow.Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://Pathfinder/Assets/Pathfinder.ico")));
                desktop.MainWindow = mainWindow;
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}