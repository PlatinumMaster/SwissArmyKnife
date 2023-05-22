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
            InitializeComponent();
            if (!Design.IsDesignMode)
                if (InitializePatcher())
                    Show();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private bool InitializePatcher() {
            try {
                baseROMConfiguration configuration = new();
                configuration.initializePatcher(PreferencesHandler.Prefs
                    .BaseRomConfiguration); // This call throws an exception if "BaseROM.yml" is not found.
                DataContext = new NewProjectViewModel(configuration, this);
                return true;
            }
            catch (Exception e) {
                MessageHandler.ErrorMessage("Initialization", e.Message);
                return false;
            }
        }
    }
}