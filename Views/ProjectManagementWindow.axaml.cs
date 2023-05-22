using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.Views {
    public class ProjectManagementWindow : Window {
        public ProjectManagementWindow() {
            Instance = this;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public static Window? Instance { get; private set; }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OpenProjectButton(object? sender, RoutedEventArgs e) {
            try {
                var result = await UI.OpenFile(this, new List<FileDialogFilter> {
                    new() {
                        Name = "Project Configuration",
                        Extensions = new List<string> {
                            "yml"
                        }
                    }
                });
                // Initialize the patcher object 
                try {
                    UI.InitializePatcher(PreferencesHandler.Prefs.BaseRomConfiguration, result);
                    Hotswap.Patcher.isPreloading = true;
                    UI.Patcher.handleROM(true);
                    new MainWindow().Show();
                    Hotswap.Patcher.isPreloading = false;
                    UI.Patcher.handleROM(false);
                    Close();
                }
                catch (Exception ex) {
                    MessageHandler.ErrorMessage("Initialization Error", ex.Message);
                }
            }
            catch (OperationCanceledException ex) {
            }
        }

        private void NewProjectButton(object? sender, RoutedEventArgs e) {
            new NewProjectWindow();
        }
    }
}