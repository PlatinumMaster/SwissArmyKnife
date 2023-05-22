using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Hotswap;
using Hotswap.Configuration;
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
        public List<GameEntry> Games => GameController.PatcherInstance.BaseROMs.Games;
        public int SelectedTab { get; set; }

        public bool CanCreate => ProjectModel.Path.Length > 0 && 
                                 ProjectModel.ProjectName.Length > 0 && 
                                 ProjectModel.ProjectROMCode.Length > 0 &&
                                 SelectedGameEntry != null;
                                 
        public StartupViewModel() {
            GameController.Init(Handlers.Preferences.Prefs.BaseROMConfigurationPath);
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
                    DialogResult Result = await Messages.YesNoMessage(Desktop, "Project Creation Successful", 
                        $"Your project \"{ProjectModel.ProjectName}\" has been created and stored at \"{ProjectRoot}\"." + "\n" 
                        + "Would you like to open it now?");
                    if (Result.GetResult.Equals("Confirm")) {
                        LoadProject(HS);
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
                        "cmproj"
                    }
                }
            };
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                string? ProjectPath = await IO.OpenFile(Desktop.MainWindow, Filters);
                if (ProjectPath != null) {
                    IProject Proj;
                    switch (Path.GetExtension(ProjectPath)) {
                        case ".yml":
                            Proj = new HotswapProject();
                            break;
                        case ".cmproj":
                            Proj = new CTRMapProject();
                            break;
                        default:
                            return;
                    }
                    Proj.Read(ProjectPath);
                    LoadProject(Proj);
                }
            }
        }

        private void LoadProject(IProject Proj) {
            if (GameController.HandleProject(Proj) && GameController.PatcherInstance != null && 
                Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                    Window? StartupWindow = Desktop.MainWindow;
                    Desktop.MainWindow = new Flow {
                        DataContext = new FlowViewModel() {
                            WindowTitle = $"{GameController.Project.Name} - SwissArmyKnife"
                        }
                    };
                    Desktop.MainWindow.Show();
                    StartupWindow.Close();
            }
        }
    }
}