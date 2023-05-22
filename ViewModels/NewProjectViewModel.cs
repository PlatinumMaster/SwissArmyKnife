using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Hotswap.Configuration;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.Utils;
using SwissArmyKnife.Avalonia.Views;

namespace SwissArmyKnife.Avalonia.ViewModels {
    public class NewProjectViewModel : ReactiveObject {
        private readonly baseROMConfiguration _baseROMConfig;
        private string _projectName;
        private string _projectPath;
        private int _selectedGame;

        public NewProjectViewModel(baseROMConfiguration baseROMs, Window instance) {
            projectName = "";
            projectPath = "";
            _baseROMConfig = baseROMs;
            this.baseROMs = new ObservableCollection<string>(_baseROMConfig.games);
            setDirectory = ReactiveCommand.Create(async () => {
                try {
                    projectPath = await UI.openFolder(instance);
                }
                catch (OperationCanceledException ex) {
                }
            });
            createProject = ReactiveCommand.Create(async () => {
                var numErrors = 0;
                var errors = new List<string>();
                if (projectName.Equals(string.Empty)) {
                    errors.Add("Invalid project name!");
                    numErrors++;
                }

                if (projectPath.Equals(string.Empty) || !Directory.Exists(projectPath)) {
                    errors.Add("Invalid project path!");
                    numErrors++;
                }

                if (selectedGame is -1) {
                    errors.Add("Game must be selected!");
                    numErrors++;
                }

                if (numErrors > 0) {
                    MessageHandler.ErrorMessage("Project Creation", string.Join('\n', errors));
                    return;
                }

                if (createProjectStructure()) {
                    instance.Close();
                    if (await promptLoadAfterCreation()) {
                        try {
                            UI.initializePatcher(PreferencesHandler.prefs.BaseROMConfiguration,
                                Path.Combine(projectPath, projectName, $"{projectName}.yml"));
                            UI.patcher.handleROM(true);
                            new MainWindow().Show();
                            UI.patcher.handleROM(false);
                        }
                        catch (Exception ex) {
                            MessageHandler.ErrorMessage("Initialization Error", ex.Message);
                        }
                    }
                }
            });
        }

        public NewProjectViewModel() {
            
        }

        public ReactiveCommand<Unit, Task> setDirectory { get; set; }
        public ReactiveCommand<Unit, Task> createProject { get; set; }
        public ObservableCollection<string> baseROMs { get; }

        public int selectedGame {
            get => _selectedGame;
            set {
                this.RaiseAndSetIfChanged(ref _selectedGame, value);
                this.RaisePropertyChanged("enableCreateButton");
            }
        }

        public bool enableCreateButton => projectName.Length > 0 && projectPath.Length > 0 && selectedGame != -1;

        public string projectName {
            get => _projectName;
            set {
                this.RaiseAndSetIfChanged(ref _projectName, value);
                this.RaisePropertyChanged("enableCreateButton");
            }
        }

        public string projectPath {
            get => _projectPath;
            set {
                this.RaiseAndSetIfChanged(ref _projectPath, value);
                this.RaisePropertyChanged("enableCreateButton");
            }
        }

        private bool createProjectStructure() {
            try {
                new Generator(projectName, projectPath, _baseROMConfig.getROMCode(baseROMs[selectedGame]));
                return true;
            }
            catch (Exception e) {
                MessageHandler.ErrorMessage("Project Creation", $"Failed to create project. \"{e.Message}\"");
                return false;
            }
        }

        private async Task<bool> promptLoadAfterCreation() {
            return await MessageHandler.YesNoMessage(
                "Project creation successful!",
                $"Your project \"{projectName}\" was successfully created at \"{Path.Combine(projectPath, projectName)}\".\nWould you like to load this project now?"
            ) == ButtonResult.Yes;
        }
    }
}