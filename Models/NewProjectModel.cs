using System;
using ReactiveUI;
using SwissArmyKnife.ViewModels;
using SwissArmyKnife.ViewModels.Base;

namespace SwissArmyKnife.Models; 

public class NewProjectModel : ReactiveObject {
    private string _projectName, _projectTitle, _projectROMCode;

    public NewProjectModel() {
        ProjectName = String.Empty;
        ProjectROMCode = String.Empty;
        Path = String.Empty;
    }
    public string ProjectName {
        get => _projectName;
        set => this.RaiseAndSetIfChanged(ref _projectName, value);
    }
        
    public string Path {
        get => _projectTitle;
        set => this.RaiseAndSetIfChanged(ref _projectTitle, value);
    }
        
    public string ProjectROMCode {
        get => _projectROMCode;
        set => this.RaiseAndSetIfChanged(ref _projectROMCode, value);
    }
}