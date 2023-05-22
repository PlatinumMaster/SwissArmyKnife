using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.ViewModels;
using SwissArmyKnife.ViewModels.Main;
using SwissArmyKnife.Views;

namespace SwissArmyKnife {
    public partial class App : Application {
        public override void Initialize() {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted() {
            Logging.InitializeLogger();
            Preferences.ReadPreferences(Path.Combine("Configuration", "Preferences.yml"));
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
                desktop.MainWindow = new StartupWindow {
                    DataContext = new StartupViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}