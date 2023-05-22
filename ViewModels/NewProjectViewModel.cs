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
        private readonly baseROMConfiguration _baseRomConfig;
        private string _projectName;
        private string _projectPath;
        private int _selectedGame;

        public NewProjectViewModel(baseROMConfiguration baseRoMs, Window instance) {
            ProjectName = "";
            ProjectPath = "";
            _baseRomConfig = baseRoMs;
            this.BaseRoMs = new ObservableCollection<string>(_baseRomConfig.games);
            SetDirectory = ReactiveCommand.Create(async () => {
                try {
                    ProjectPath = await UI.OpenFolder(instance);
                }
                catch (OperationCanceledException ex) {
                }
            });
            CreateProject = ReactiveCommand.Create(async () => {
                var numErrors = 0;
                var errors = new List<string>();
                if (ProjectName.Equals(string.Empty)) {
                    errors.Add("Invalid project name!");
                    numErrors++;
                }

                if (ProjectPath.Equals(string.Empty) || !Directory.Exists(ProjectPath)) {
                    errors.Add("Invalid project path!");
                    numErrors++;
                }

                if (SelectedGame is -1) {
                    errors.Add("Game must be selected!");
                    numErrors++;
                }

                if (numErrors > 0) {
                    MessageHandler.ErrorMessage("Project Creation", string.Join('\n', errors));
                    return;
                }

                if (CreateProjectStructure()) {
                    instance.Close();
                    if (await PromptLoadAfterCreation()) {
                        try {
                            UI.InitializePatcher(PreferencesHandler.Prefs.BaseRomConfiguration,
                                Path.Combine(ProjectPath, ProjectName, $"{ProjectName}.yml"));
                            UI.Patcher.handleROM(true);
                            new MainWindow().Show();
                            UI.Patcher.handleROM(false);
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

        public ReactiveCommand<Unit, Task> SetDirectory { get; set; }
        public ReactiveCommand<Unit, Task> CreateProject { get; set; }
        public ObservableCollection<string> BaseRoMs { get; }

        public int SelectedGame {
            get => _selectedGame;
            set {
                this.RaiseAndSetIfChanged(ref _selectedGame, value);
                this.RaisePropertyChanged("enableCreateButton");
            }
        }

        public bool EnableCreateButton => ProjectName.Length > 0 && ProjectPath.Length > 0 && SelectedGame != -1;

        public string ProjectName {
            get => _projectName;
            set {
                this.RaiseAndSetIfChanged(ref _projectName, value);
                this.RaisePropertyChanged("enableCreateButton");
            }
        }

        public string ProjectPath {
            get => _projectPath;
            set {
                this.RaiseAndSetIfChanged(ref _projectPath, value);
                this.RaisePropertyChanged("enableCreateButton");
            }
        }

        private bool CreateProjectStructure() {
            try {
                new Generator(ProjectName, ProjectPath, _baseRomConfig.getROMCode(BaseRoMs[SelectedGame]));
                return true;
            }
            catch (Exception e) {
                MessageHandler.ErrorMessage("Project Creation", $"Failed to create project. \"{e.Message}\"");
                return false;
            }
        }

        private async Task<bool> PromptLoadAfterCreation() {
            return await MessageHandler.YesNoMessage(
                "Project creation successful!",
                $"Your project \"{ProjectName}\" was successfully created at \"{Path.Combine(ProjectPath, ProjectName)}\".\nWould you like to load this project now?"
            ) == ButtonResult.Yes;
        }
    }
}