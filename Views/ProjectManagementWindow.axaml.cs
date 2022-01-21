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
            instance = this;
            initializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public static Window? instance { get; private set; }

        private void initializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private async void openProjectButton(object? sender, RoutedEventArgs e) {
            try {
                var result = await UI.openFile(this, new List<FileDialogFilter> {
                    new() {
                        Name = "Project Configuration",
                        Extensions = new List<string> {
                            "yml"
                        }
                    }
                });
                // Initialize the patcher object 
                try {
                    UI.initializePatcher(PreferencesHandler.prefs.baseROMConfigurationPath, result);
                    Hotswap.Patcher.isPreloading = true;
                    UI.patcher.handleROM(true);
                    new MainWindow().Show();
                    Hotswap.Patcher.isPreloading = false;
                    UI.patcher.handleROM(false);
                    Close();
                }
                catch (Exception ex) {
                    MessageHandler.errorMessage("Initialization Error", ex.Message);
                }
            }
            catch (OperationCanceledException ex) {
            }
        }

        private void newProjectButton(object? sender, RoutedEventArgs e) {
            new NewProjectWindow();
        }
    }
}