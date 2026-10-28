using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace NotesApp;

public partial class MainWindow : Window
{
    private string savedText = "";

    public MainWindow()
    {
        InitializeComponent();

        NotesTextBox.TextChanged += (_, _) =>
        {
            UpdateWordCount();
            StatusLabel.Text =
                (NotesTextBox.Text ?? "") == savedText
                    ? "Saved"
                    : "Unsaved changes";
        };

        UpdateWordCount();
    }

    private void UpdateWordCount()
    {
        string text = NotesTextBox.Text ?? "";
        string trimmedText = text.Trim();

        int count = trimmedText.Length == 0
            ? 0
            : trimmedText.Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries).Length;

        WordCountLabel.Text = $"Words: {count}";
    }

    private async void OpenButton_Click(
        object? sender, RoutedEventArgs e)
    {
        try
        {
            var storage = TopLevel.GetTopLevel(this)?.StorageProvider;

            if (storage is null)
            {
                await ShowMessageAsync("Error", "File picker is unavailable.");
                return;
            }

            var files = await storage.OpenFilePickerAsync(
                new FilePickerOpenOptions
                {
                    Title = "Open Notes",
                    AllowMultiple = false,
                    FileTypeFilter =
                    [
                        new FilePickerFileType("Text files")
                        {
                            Patterns = ["*.txt"]
                        }
                    ]
                });

            if (files.Count == 0)
                return;

            await using var stream = await files[0].OpenReadAsync();
            using var reader = new StreamReader(stream);

            string content = await reader.ReadToEndAsync();

            NotesTextBox.Text = content;
            savedText = content;
            StatusLabel.Text = "File opened successfully";
            UpdateWordCount();
        }
        catch (Exception ex)
        {
            await ShowMessageAsync(
                "Open Error",
                $"Could not open the file.\n{ex.Message}");
        }
    }

    private async void SaveButton_Click(
        object? sender, RoutedEventArgs e)
    {
        try
        {
            var storage = TopLevel.GetTopLevel(this)?.StorageProvider;

            if (storage is null)
            {
                await ShowMessageAsync("Error", "File picker is unavailable.");
                return;
            }

            var file = await storage.SaveFilePickerAsync(
                new FilePickerSaveOptions
                {
                    Title = "Save Notes",
                    SuggestedFileName = "notes.txt",
                    DefaultExtension = "txt",
                    FileTypeChoices =
                    [
                        new FilePickerFileType("Text files")
                        {
                            Patterns = ["*.txt"]
                        }
                    ]
                });

            if (file is null)
                return;

            await using var stream = await file.OpenWriteAsync();

            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(NotesTextBox.Text ?? "");
                await writer.FlushAsync();
            }

            savedText = NotesTextBox.Text ?? "";
            StatusLabel.Text = "File saved successfully";
        }
        catch (Exception ex)
        {
            await ShowMessageAsync(
                "Save Error",
                $"Could not save the file.\n{ex.Message}");
        }
    }

    private async Task ShowMessageAsync(
        string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 380,
            Height = 180,
            CanResize = false,
            WindowStartupLocation =
                WindowStartupLocation.CenterOwner
        };

        var text = new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(12)
        };

        var okButton = new Button
        {
            Content = "OK",
            Width = 90,
            HorizontalAlignment =
                Avalonia.Layout.HorizontalAlignment.Center
        };

        okButton.Click += (_, _) => dialog.Close();

        dialog.Content = new StackPanel
        {
            Margin = new Avalonia.Thickness(12),
            Spacing = 12,
            Children = { text, okButton }
        };

        await dialog.ShowDialog(this);
    }
}
