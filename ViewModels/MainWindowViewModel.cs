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
        private static Window instance;
        public MainWindowViewModel(Window instance) {
            openProject = ReactiveCommand.Create(async () => {
                try {
                    var result = await UI.openFile(instance, new List<FileDialogFilter> {
                        new() {
                            Name = "Project Configuration",
                            Extensions = new List<string> {
                                "yml"
                            }
                        }
                    });
                    UI.initializePatcher(PreferencesHandler.prefs.baseROMConfigurationPath, result);
                }
                catch (OperationCanceledException ex) {
                }
            });
            exportROM = ReactiveCommand.Create(async () => {
                try {
                    var result = await UI.saveFile(instance, new List<FileDialogFilter> {
                        new() {
                            Name = "Nintendo DS ROM",
                            Extensions = new List<string> {
                                "nds"
                            }
                        }
                    });
                    UI.patcher.patchAndSerialize(result);
                }
                catch (OperationCanceledException ex) {
                }
            });
        }

        public ReactiveCommand<Unit, Task> openProject { get; }
        public ReactiveCommand<Unit, Unit> saveProject { get; }
        public ReactiveCommand<Unit, Task> exportROM { get; }
    }
}