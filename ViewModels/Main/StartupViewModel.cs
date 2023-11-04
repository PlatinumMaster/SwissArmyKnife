using System.Collections.Generic;
using System.IO;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Hotswap.Configuration;
using Hotswap.Project;
using Material.Dialog;
using ReactiveUI;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.Models;
using SwissArmyKnife.ViewModels.Base;
using SwissArmyKnife.Views;

namespace SwissArmyKnife.ViewModels.Main {
    public class StartupViewModel : ViewModelBase {
        public NewProjectModel ProjectModel { get; private set; }
        public ReactiveCommand<Unit, Unit> MakeProject { get; }
        public ReactiveCommand<Unit, Unit> OpenProject { get; }
        public ReactiveCommand<Unit, Unit> SelectPath { get; }
        public GameEntry SelectedGameEntry { get; set; }
        public List<GameEntry> Games => GameWork.Patcher.BaseROMs.Games;
        public int SelectedTab { get; set; }

        public bool CanCreate => ProjectModel.Path.Length > 0 && 
                                 ProjectModel.ProjectName.Length > 0 && 
                                 ProjectModel.ProjectROMCode.Length > 0 &&
                                 SelectedGameEntry != null;
                                 
        public StartupViewModel() {
            GameWork.Init(Handlers.Preferences.Prefs.BaseROMConfigurationPath);
            ProjectModel = new NewProjectModel();
            SelectPath = ReactiveCommand.Create(OnPathSelect);
            MakeProject = ReactiveCommand.Create(OnCreateNewProject);
            OpenProject = ReactiveCommand.Create(OnOpenProject);
            SelectedTab = 1;
        }

        private async void OnPathSelect() {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                string Res = await IO.OpenFolder(Desktop.MainWindow);
                if (Res != null) {
                    ProjectModel.Path = Res;
                }
            }
        }
        
        private async void OnCreateNewProject() {
            if (CanCreate) {
                string? ProjectRoot = Path.Combine(ProjectModel.Path, ProjectModel.ProjectName);
                HotswapProject HS = new HotswapProject() {
                    Base = SelectedGameEntry.Path,
                    Path = ProjectRoot,
                    Name = ProjectModel.ProjectName,
                    ROMCode = ProjectModel.ProjectROMCode,
                    VFS = "VFS",
                    User = "User",
                };
                HS.Generate(ProjectRoot);
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                    DialogResult Result = await Messages.YesNo(Desktop, "Project Creation Successful", 
                        $"Your project \"{ProjectModel.ProjectName}\" has been created and stored at \"{ProjectRoot}\"." + "\n" 
                        + "Would you like to open it now?");
                    if (Result.GetResult.Equals("Confirm")) {
                        LoadProject(Desktop, HS);
                    }
                }
            }
        }
        
        private async void OnOpenProject() {
            List<FileDialogFilter> Filters = new List<FileDialogFilter> {
                new() {
                    Name = "Project Configuration",
                    Extensions = new List<string> {
                        "yml",
                    }
                }
            };
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                string? ProjectPath = await IO.OpenFile(Desktop.MainWindow, Filters);
                if (ProjectPath != null) {
                    IProject Proj;
                    switch (Path.GetExtension(ProjectPath)) {
                        case ".yml":
                            Proj = new HotswapProject(ProjectPath);
                            break;
                        case ".cmproj":
                            Proj = new CTRMapProject(ProjectPath);
                            break;
                        default:
                            return;
                    }
                    LoadProject(Desktop, Proj);
                }
            }
        }

        private async void LoadProject(IClassicDesktopStyleApplicationLifetime Desktop, IProject Proj) {
            if (GameWork.HandleProject(Proj) && GameWork.Patcher != null) {
                DialogResult ScriptCommandDownloadPrompt = await Messages.YesNo(Desktop, "Update Script Commands", "Would you like to fetch the latest script commands? (Make sure you have internet before doing this).");
                if (ScriptCommandDownloadPrompt.GetResult.Equals("Confirm")) {
                    if (!await Commands.Fetch()) {
                        Messages.Error(Desktop, "Fetching Error", "An error occurred when trying to fetch the latest script commands from the repository.\nThe previous version will be used.");
                    }
                }
                Window? StartupWindow = Desktop.MainWindow;
                Desktop.MainWindow = new Flow {
                    DataContext = new FlowViewModel() {
                        WindowTitle = $"{GameWork.Project.Name} - SwissArmyKnife"
                    }
                };
                Desktop.MainWindow.Show();
                StartupWindow.Close();
            } else {
                Messages.Error(Desktop, "Project Failed to Load", "An error occurred when loading your project. Make sure it is valid, then try again.");
            }
        }
    }
}