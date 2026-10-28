
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SplashScreenApp;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private async void LoginButton_Click(
        object? sender, RoutedEventArgs e)
    {
        var message = new Window
        {
            Title = "Login",
            Width = 280,
            Height = 120,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new TextBlock
            {
                Text = "Login button clicked!",
                HorizontalAlignment =
                    Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment =
                    Avalonia.Layout.VerticalAlignment.Center
            }
        };

        await message.ShowDialog(this);
    }
}
