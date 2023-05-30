using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.ViewModels.Main;
using SwissArmyKnife.Avalonia.Views;

namespace SwissArmyKnife.Avalonia {
    public partial class App : Application {
        public override void Initialize() {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted() {
            Logging.InitializeLogger();
            Net.Initialize();
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