using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using BeaterLibrary.Formats.Maps;
using BeaterLibrary.Formats.Nitro;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Utils;
using SwissArmyKnife.Avalonia.Views;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors; 

public class MapEditorViewModel : ViewModelTemplate {
    private int _selectedIndex;
    private MapContainer _currentMapContainer;

    public MapContainer CurrentMapContainer {
        get => _currentMapContainer;
        private set {
            this.RaiseAndSetIfChanged(ref _currentMapContainer, value);
            this.RaisePropertyChanged(nameof(CurrentMapContainerType));
            this.RaisePropertyChanged(nameof(HasPerms));
            this.RaisePropertyChanged(nameof(HasPerms2));
            this.RaisePropertyChanged(nameof(HasBldPos));
            this.RaisePropertyChanged(nameof(ModelName));
            this.RaisePropertyChanged();
        }
    }

    public int CurrentMapContainerType {
        get {
            switch (CurrentMapContainer.containerType) {
                case MapContainer.MagicLabels.Ng:
                    return 0;
                case MapContainer.MagicLabels.Rd:
                    return 1;
                case MapContainer.MagicLabels.Wb:
                    return 2;
                case MapContainer.MagicLabels.Gc:
                    return 3;
                default:
                    return -1;
            }
        }
    }

    public override int SelectedIndex {
        get => _selectedIndex;
        set => OnIndexChange(value);
    }

    public bool HasPerms => CurrentMapContainer.permissions.Length > 0;
    public bool HasPerms2 => CurrentMapContainer.permissions2.Length > 0;
    public bool HasBldPos => CurrentMapContainer.buildingPositions.Length > 0;
    public string ModelName => CurrentMapContainer.model.name;

    public ReactiveCommand<Unit, Task> ImportModel { get; }
    public ReactiveCommand<Unit, Task> ExportModel { get; }
    public ReactiveCommand<Unit, Task> ImportPerms { get; }
    public ReactiveCommand<Unit, Task> ExportPerms { get; }
    public ReactiveCommand<Unit, Task> RemovePerms { get; }
    public ReactiveCommand<Unit, Task> ImportPerms2 { get; }
    public ReactiveCommand<Unit, Task> ExportPerms2 { get; }
    public ReactiveCommand<Unit, Task> RemovePerms2 { get; }
    public ReactiveCommand<Unit, Task> ImportBuildingPos { get; }
    public ReactiveCommand<Unit, Task> ExportBuildingPos { get; }
    public ReactiveCommand<Unit, Task> RemoveBuildingPos { get; }
    public ReactiveCommand<Unit, Unit> LoadMapContainer { get; }

    public ObservableCollection<string> MapTypes { get; }

    public MapEditorViewModel() {
        LoadMapContainer = ReactiveCommand.Create(() => {
            CurrentMapContainer = new MapContainer(UI.Patcher.fetchFileFromNarc(UI.GameInfo.maps, SelectedIndex));
        });
        MapTypes = new() {
            "NG", "RD", "WB", "GC"
        };
        ImportModel = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.OpenFile(ProjectManagementWindow.Instance, new List<FileDialogFilter>() {
                    new FileDialogFilter() {
                        Name = "NITRO System Binary Model",
                        Extensions = new List<string>() {
                            "nsbmd",
                            "bmd0",
                            "bin"
                        }
                    }
                });
                CurrentMapContainer.model = new NitroSystemBinaryModel(File.ReadAllBytes(path));
                this.RaisePropertyChanged(nameof(ModelName));
            }
            catch (OperationCanceledException ex) {
            }
        });
        ExportModel = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.SaveFile(ProjectManagementWindow.Instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "NITRO System Binary Model",
                        Extensions = new List<string>() {
                            "nsbmd",
                            "bmd0",
                            "bin"
                        }
                    }
                });
                File.WriteAllBytes(path, CurrentMapContainer.model.data);
            }
            catch (OperationCanceledException ex) {
            }
        });        
        ImportPerms = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.OpenFile(ProjectManagementWindow.Instance, new List<FileDialogFilter>() {
                    new FileDialogFilter() {
                        Name = "Permissions",
                        Extensions = new List<string>() {
                            "per",
                            "bin"
                        }
                    }
                });
                CurrentMapContainer.permissions = File.ReadAllBytes(path);
                CurrentMapContainer.updateContainerType();
                this.RaisePropertyChanged(nameof(CurrentMapContainerType));
                this.RaisePropertyChanged(nameof(HasPerms));
            }
            catch (OperationCanceledException ex) {
            }
        });
        ExportPerms = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.SaveFile(ProjectManagementWindow.Instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "Permissions",
                        Extensions = new List<string>() {
                            "per",
                            "bin"
                        }
                    }
                });
                File.WriteAllBytes(path, CurrentMapContainer.permissions);
            }
            catch (OperationCanceledException ex) {
            }
        });  
        RemovePerms = ReactiveCommand.Create(async () => {
            CurrentMapContainer.permissions = Array.Empty<byte>();
            CurrentMapContainer.updateContainerType();
            this.RaisePropertyChanged(nameof(CurrentMapContainerType));
            this.RaisePropertyChanged(nameof(HasPerms));
        }); 
        ImportPerms2 = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.OpenFile(ProjectManagementWindow.Instance, new List<FileDialogFilter>() {
                    new FileDialogFilter() {
                        Name = "Permissions",
                        Extensions = new List<string>() {
                            "per",
                            "bin"
                        }
                    }
                });
                CurrentMapContainer.permissions2 = File.ReadAllBytes(path);
                CurrentMapContainer.updateContainerType();
                this.RaisePropertyChanged(nameof(CurrentMapContainerType));
                this.RaisePropertyChanged(nameof(HasPerms2));
            }
            catch (OperationCanceledException ex) {
            }
        });
        ExportPerms2 = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.SaveFile(ProjectManagementWindow.Instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "Permissions",
                        Extensions = new List<string>() {
                            "per",
                            "bin"
                        }
                    }
                });
                File.WriteAllBytes(path, CurrentMapContainer.permissions2);
            }
            catch (OperationCanceledException ex) {
            }
        });  
        RemovePerms2 = ReactiveCommand.Create(async () => {
            CurrentMapContainer.permissions2 = Array.Empty<byte>();
            CurrentMapContainer.updateContainerType();
            this.RaisePropertyChanged(nameof(CurrentMapContainerType));
            this.RaisePropertyChanged(nameof(HasPerms2));
        }); 
        ImportBuildingPos = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.OpenFile(ProjectManagementWindow.Instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "Building Positions",
                        Extensions = new List<string>() {
                            "bld",
                            "bin"
                        }
                    }
                });
                CurrentMapContainer.buildingPositions = File.ReadAllBytes(path);
                this.RaisePropertyChanged(nameof(HasBldPos));
            }
            catch (OperationCanceledException ex) {
            }
        });
        ExportBuildingPos = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.SaveFile(ProjectManagementWindow.Instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "Building Positions",
                        Extensions = new List<string>() {
                            "bld",
                            "bin"
                        }
                    }
                });
                File.WriteAllBytes(path, CurrentMapContainer.buildingPositions);
            }
            catch (OperationCanceledException ex) {
            }
        });  
        RemoveBuildingPos = ReactiveCommand.Create(async () => {
            CurrentMapContainer.buildingPositions = Array.Empty<byte>();
            this.RaisePropertyChanged(nameof(HasBldPos));
        }); 
        SelectedIndex = 0;
        CurrentMapContainer = new MapContainer(UI.Patcher.fetchFileFromNarc(UI.GameInfo.maps, SelectedIndex));
    }
    
    public override void OnAddNew() {
        throw new System.NotImplementedException();
    }

    public override void OnRemoveSelected(int index) {
        throw new System.NotImplementedException();
    }

    public override void OnIndexChange(int newValue) {
        if (newValue >= 0 && newValue < UI.Patcher.getNarcEntryCount(UI.GameInfo.maps)) {
            this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
            this.RaisePropertyChanged(nameof(SelectedIndex));
        }
    }

    public override void OnSaveChanges() {
        UI.Patcher.saveToNarcFolder(UI.GameInfo.maps, SelectedIndex, x => CurrentMapContainer.serialize(x));
    }
}