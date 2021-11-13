using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace SwissArmyKnife.Avalonia.Views
{
    public class ProjectManagementWindow : Window
    {
        public ProjectManagementWindow()
        {
            Instance = this;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public static Window? Instance { get; private set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OpenProjectButton(object? Sender, RoutedEventArgs E)
        {
            try
            {
                string[] Result = await new OpenFileDialog {Filters = new List<FileDialogFilter>()}.ShowAsync(this);
                if (Result.Length > 0)
                {
                    // Initialize the patcher object 
                    UIUtil.InitializePatcher(Result.First());
                    new MainWindow().Show();
                    Close();
                }
            }
            catch (Exception e)
            {
                MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Error",
                        ContentMessage = $"{e.Message}\n{e.StackTrace}",
                        Icon = MessageBox.Avalonia.Enums.Icon.Error,
                        Style = Style.None
                    }).Show();
            }
        }

        private void NewProjectButton(object? Sender, RoutedEventArgs E)
        {
            new NewProjectWindow().Show();
        }
    }
}