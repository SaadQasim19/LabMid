
using System;
using Avalonia.Controls;
using Avalonia.Threading;

namespace SplashScreenApp;

public partial class SplashWindow : Window
{
    private readonly DispatcherTimer _timer;
    private int _progress = 0;
    private readonly LoginWindow _loginWindow;

    public SplashWindow(LoginWindow loginWindow)
    {
        InitializeComponent();

        _loginWindow = loginWindow;

        // Create the timer.
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(50)
        };

        // Run this method on every timer tick.
        _timer.Tick += Timer_Tick;

        // Start the timer.
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        // Increase progress by 2 on each tick.
        _progress += 2;

        // Update progress bar and percentage label.
        LoadingProgress.Value = _progress;
        ProgressLabel.Text = $"{_progress}%";

        // When progress reaches 100%.
        if (_progress >= 100)
        {
            // Stop the timer.
            _timer.Stop();

            // Open the login form.
            _loginWindow.Show();

            // Hide the splash screen.
            Hide();
        }
    }
}
