
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Threading.Tasks;

namespace AvaloniaTest;

public partial class SplashWindow : Window
{
    public SplashWindow()
    {
        InitializeComponent();

        Opened += SplashWindow_Opened;
    }

    private async void SplashWindow_Opened(
        object? sender,
        EventArgs e)
    {
        await Task.Delay(3000);

        if (Application.Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();

            desktop.MainWindow = mainWindow;

            mainWindow.Show();

            Close();
        }
    }
}
