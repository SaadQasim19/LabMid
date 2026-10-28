Haha 😂 samajh gaya! We need to create a Notes Application as a task, not just answer the lab question. We'll create a working Avalonia app in your existing Ubuntu/.NET environment.

## 1. Create the project

Since you already have an Avalonia project, we can use it. Open the terminal:

```
cd ~/Desktop/AdvProgLab/AvaloniaTest
code .
```

We'll use these files:

- `MainWindow.axaml` — application UI
- `MainWindow.axaml.cs` — application logic, file handling, and word count

## 2. Design the application

## Notes App

Write your notes here...

Start typing your notes in the text editor.

&#x20;OPEN

&#x20;SAVE

Words: 0

UI preview — a simple notes editor with file operations and a live word count.

## 3. `MainWindow.axaml`

Replace the complete contents of `MainWindow.axaml` with:

```

<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        x:Class="AvaloniaTest.MainWindow"
        Title="Notes App"
        Width="750"
        Height="520"
        MinWidth="500"
        MinHeight="350"
        WindowStartupLocation="CenterScreen">

    <Grid RowDefinitions="Auto,*,Auto"
          Margin="20"
          RowSpacing="12">

        <TextBlock Grid.Row="0"
                   Text="My Notes"
                   FontSize="28"
                   FontWeight="Bold"/>

        <TextBox x:Name="NotesTextBox"
                 Grid.Row="1"
                 AcceptsReturn="True"
                 TextWrapping="Wrap"
                 VerticalContentAlignment="Top"
                 PlaceholderText="Write your notes here..."
                 ScrollViewer.VerticalScrollBarVisibility="Auto"/>

        <Grid Grid.Row="2"
              ColumnDefinitions="Auto,Auto,*,Auto"
              ColumnSpacing="10">

            <Button Grid.Column="0"
                    Content="OPEN"
                    MinWidth="100"
                    Click="OpenButton_Click"/>

            <Button Grid.Column="1"
                    Content="SAVE"
                    MinWidth="100"
                    Click="SaveButton_Click"/>

            <TextBlock x:Name="StatusLabel"
                       Grid.Column="2"
                       VerticalAlignment="Center"
                       Text="Ready"/>

            <TextBlock x:Name="WordCountLabel"
                       Grid.Column="3"
                       VerticalAlignment="Center"
                       Text="Words: 0"/>
        </Grid>

    </Grid>
</Window>

```

## 4. `MainWindow.axaml.cs`

Replace the complete contents of `MainWindow.axaml.cs` with:

```

using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace AvaloniaTest;

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

```

## 5. Run the application

From your terminal:

```
cd ~/Desktop/AdvProgLab/AvaloniaTest
dotnet build
dotnet run
```

This version includes the multiline editor, Open and Save file dialogs, live word count, status label, and error messages. It keeps the existing project and namespace intact.

One important note: this is the basic task version. It does not yet prompt you to save unsaved changes when exiting; we can add that after confirming this version builds and runs successfully.