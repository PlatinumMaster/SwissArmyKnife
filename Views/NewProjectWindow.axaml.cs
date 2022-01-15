using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Hotswap.Configuration;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.ViewModels;

namespace SwissArmyKnife.Avalonia.Views {
    public class NewProjectWindow : ReactiveWindow<NewProjectViewModel> {
        public NewProjectWindow() {
            initializeComponent();
            if (!Design.IsDesignMode)
                if (initializePatcher())
                    Show();
        }

        private void initializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private bool initializePatcher() {
            try {
                baseROMConfiguration configuration = new();
                configuration.initializePatcher(PreferencesHandler.prefs
                    .baseROMConfigurationPath); // This call throws an exception if "BaseROM.yml" is not found.
                DataContext = new NewProjectViewModel(configuration, this);
                return true;
            }
            catch (Exception e) {
                MessageHandler.errorMessage("Initialization", e.Message);
                return false;
            }
        }
    }
}