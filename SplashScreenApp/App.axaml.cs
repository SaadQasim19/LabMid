
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SplashScreenApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is
            IClassicDesktopStyleApplicationLifetime desktop)
        {
            var loginWindow = new LoginWindow();

            // The login form controls the application lifetime.
            desktop.MainWindow = loginWindow;

            // Show the splash screen first.
            var splashWindow = new SplashWindow(loginWindow);
            splashWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
