using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace AvaloniaTest;

public partial class MainWindow : Window
{
    private int failedAttempts = 0;
    private const int MaxAttempts = 3;

    private const string StoredUsername = "student";
    private const string StoredPassword = "12345";

    public MainWindow()
    {
        InitializeComponent();
        UsernameTextBox.Focus();
    }

   private void ShowPasswordCheckBox_IsCheckedChanged(
    object? sender, RoutedEventArgs e)
{
    if (ShowPasswordCheckBox.IsChecked == true)
    {
        PasswordTextBox.PasswordChar = '\0';
    }
    else
    {
        PasswordTextBox.PasswordChar = '●';
    }
}

    private async void LoginButton_Click(
        object? sender, RoutedEventArgs e)
    {
        string username = UsernameTextBox.Text?.Trim() ?? "";
        string password = PasswordTextBox.Text ?? "";

        // Empty fields do not count as failed attempts.
        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrEmpty(password))
        {
            await ShowMessageAsync(
                "Validation",
                "Please enter both username and password.");

            return;
        }

        // Check stored credentials.
        if (username == StoredUsername &&
            password == StoredPassword)
        {
            DateTime loginTime = DateTime.Now;

            var dashboard =
                new DashboardWindow(username, loginTime);

            dashboard.Closed += (_, _) => Show();

            Hide();
            dashboard.Show();

            return;
        }

        // Wrong credentials: count one failed attempt.
        failedAttempts++;

        int remainingAttempts = MaxAttempts - failedAttempts;

        if (remainingAttempts > 0)
        {
            AttemptsLabel.Text =
                $"{remainingAttempts} attempts remaining";

            await ShowMessageAsync(
                "Login Failed",
                $"Invalid username or password.\n" +
                $"{remainingAttempts} attempts remaining.");
        }
        else
        {
            AttemptsLabel.Text = "Login locked";
            LoginButton.IsEnabled = false;

            await ShowMessageAsync(
                "Account Locked",
                "Three failed attempts. Login is now locked.");
        }
    }

    private void ClearButton_Click(
        object? sender, RoutedEventArgs e)
    {
        UsernameTextBox.Text = "";
        PasswordTextBox.Text = "";

        ShowPasswordCheckBox.IsChecked = false;
        PasswordTextBox.PasswordChar = '●';

        UsernameTextBox.Focus();
    }

    private async Task ShowMessageAsync(
        string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 340,
            Height = 190,
            CanResize = false,
            WindowStartupLocation =
                WindowStartupLocation.CenterOwner
        };

        var messageText = new TextBlock
        {
            Text = message,
            TextWrapping =
                Avalonia.Media.TextWrapping.Wrap,
            HorizontalAlignment =
                HorizontalAlignment.Center,
            VerticalAlignment =
                VerticalAlignment.Center,
            Margin = new Avalonia.Thickness(10)
        };

        var okButton = new Button
        {
            Content = "OK",
            Width = 90,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        okButton.Click += (_, _) => dialog.Close();

        dialog.Content = new StackPanel
        {
            Margin = new Avalonia.Thickness(15),
            Spacing = 15,
            VerticalAlignment = VerticalAlignment.Center,
            Children =
            {
                messageText,
                okButton
            }
        };

        await dialog.ShowDialog(this);
    }
}