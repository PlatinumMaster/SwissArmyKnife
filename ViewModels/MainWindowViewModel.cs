using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels {
    public class MainWindowViewModel : ReactiveObject {
        private static Window _instance;
        public MainWindowViewModel(Window instance) {
            OpenProject = ReactiveCommand.Create(async () => {
                try {
                    var result = await UI.OpenFile(instance, new List<FileDialogFilter> {
                        new() {
                            Name = "Project Configuration",
                            Extensions = new List<string> {
                                "yml"
                            }
                        }
                    });
                    UI.InitializePatcher(PreferencesHandler.Prefs.BaseRomConfiguration, result);
                }
                catch (OperationCanceledException ex) {
                }
            });
            ExportRom = ReactiveCommand.Create(async () => {
                try {
                    var result = await UI.SaveFile(instance, new List<FileDialogFilter> {
                        new() {
                            Name = "Nintendo DS ROM",
                            Extensions = new List<string> {
                                "nds"
                            }
                        }
                    });
                    UI.Patcher.patchAndSerialize(result);
                }
                catch (OperationCanceledException ex) {
                }
            });
        }

        public ReactiveCommand<Unit, Task> OpenProject { get; }
        public ReactiveCommand<Unit, Unit> SaveProject { get; }
        public ReactiveCommand<Unit, Task> ExportRom { get; }
    }
}