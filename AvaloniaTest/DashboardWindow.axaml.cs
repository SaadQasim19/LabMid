using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaTest;

public partial class DashboardWindow : Window
{
    public DashboardWindow(string username, DateTime loginTime)
    {
        InitializeComponent();

        WelcomeLabel.Text = $"Welcome, {username}";

        LoginTimeLabel.Text =
            $"Login date and time: {loginTime:dd-MM-yyyy hh:mm:ss tt}";
    }

    private void LogoutButton_Click(
        object? sender, RoutedEventArgs e)
    {
        Close();
    }
}