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

    public MapContainer currentMapContainer {
        get => _currentMapContainer;
        private set {
            this.RaiseAndSetIfChanged(ref _currentMapContainer, value);
            this.RaisePropertyChanged(nameof(currentMapContainerType));
            this.RaisePropertyChanged(nameof(hasPerms));
            this.RaisePropertyChanged(nameof(hasPerms2));
            this.RaisePropertyChanged(nameof(hasBldPos));
            this.RaisePropertyChanged(nameof(modelName));
            this.RaisePropertyChanged();
        }
    }

    public int currentMapContainerType {
        get {
            switch (currentMapContainer.containerType) {
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

    public override int selectedIndex {
        get => _selectedIndex;
        set => onIndexChange(value);
    }

    public bool hasPerms => currentMapContainer.permissions.Length > 0;
    public bool hasPerms2 => currentMapContainer.permissions2.Length > 0;
    public bool hasBldPos => currentMapContainer.buildingPositions.Length > 0;
    public string modelName => currentMapContainer.model.name;

    public ReactiveCommand<Unit, Task> importModel { get; }
    public ReactiveCommand<Unit, Task> exportModel { get; }
    public ReactiveCommand<Unit, Task> importPerms { get; }
    public ReactiveCommand<Unit, Task> exportPerms { get; }
    public ReactiveCommand<Unit, Task> removePerms { get; }
    public ReactiveCommand<Unit, Task> importPerms2 { get; }
    public ReactiveCommand<Unit, Task> exportPerms2 { get; }
    public ReactiveCommand<Unit, Task> removePerms2 { get; }
    public ReactiveCommand<Unit, Task> importBuildingPos { get; }
    public ReactiveCommand<Unit, Task> exportBuildingPos { get; }
    public ReactiveCommand<Unit, Task> removeBuildingPos { get; }

    public ObservableCollection<string> mapTypes { get; }

    public MapEditorViewModel() {
        mapTypes = new() {
            "NG", "RD", "WB", "GC"
        };
        importModel = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.openFile(ProjectManagementWindow.instance, new List<FileDialogFilter>() {
                    new FileDialogFilter() {
                        Name = "NITRO System Binary Model",
                        Extensions = new List<string>() {
                            "nsbmd",
                            "bmd0",
                            "bin"
                        }
                    }
                });
                currentMapContainer.model = new NitroSystemBinaryModel(File.ReadAllBytes(path));
                this.RaisePropertyChanged(nameof(modelName));
            }
            catch (OperationCanceledException ex) {
            }
        });
        exportModel = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.saveFile(ProjectManagementWindow.instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "NITRO System Binary Model",
                        Extensions = new List<string>() {
                            "nsbmd",
                            "bmd0",
                            "bin"
                        }
                    }
                });
                File.WriteAllBytes(path, currentMapContainer.model.data);
            }
            catch (OperationCanceledException ex) {
            }
        });        
        importPerms = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.openFile(ProjectManagementWindow.instance, new List<FileDialogFilter>() {
                    new FileDialogFilter() {
                        Name = "Permissions",
                        Extensions = new List<string>() {
                            "per",
                            "bin"
                        }
                    }
                });
                currentMapContainer.permissions = File.ReadAllBytes(path);
                currentMapContainer.updateContainerType();
                this.RaisePropertyChanged(nameof(currentMapContainerType));
                this.RaisePropertyChanged(nameof(hasPerms));
            }
            catch (OperationCanceledException ex) {
            }
        });
        exportPerms = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.saveFile(ProjectManagementWindow.instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "Permissions",
                        Extensions = new List<string>() {
                            "per",
                            "bin"
                        }
                    }
                });
                File.WriteAllBytes(path, currentMapContainer.permissions);
            }
            catch (OperationCanceledException ex) {
            }
        });  
        removePerms = ReactiveCommand.Create(async () => {
            currentMapContainer.permissions = Array.Empty<byte>();
            currentMapContainer.updateContainerType();
            this.RaisePropertyChanged(nameof(currentMapContainerType));
            this.RaisePropertyChanged(nameof(hasPerms));
        }); 
        importPerms2 = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.openFile(ProjectManagementWindow.instance, new List<FileDialogFilter>() {
                    new FileDialogFilter() {
                        Name = "Permissions",
                        Extensions = new List<string>() {
                            "per",
                            "bin"
                        }
                    }
                });
                currentMapContainer.permissions2 = File.ReadAllBytes(path);
                currentMapContainer.updateContainerType();
                this.RaisePropertyChanged(nameof(currentMapContainerType));
                this.RaisePropertyChanged(nameof(hasPerms2));
            }
            catch (OperationCanceledException ex) {
            }
        });
        exportPerms2 = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.saveFile(ProjectManagementWindow.instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "Permissions",
                        Extensions = new List<string>() {
                            "per",
                            "bin"
                        }
                    }
                });
                File.WriteAllBytes(path, currentMapContainer.permissions2);
            }
            catch (OperationCanceledException ex) {
            }
        });  
        removePerms2 = ReactiveCommand.Create(async () => {
            currentMapContainer.permissions2 = Array.Empty<byte>();
            currentMapContainer.updateContainerType();
            this.RaisePropertyChanged(nameof(currentMapContainerType));
            this.RaisePropertyChanged(nameof(hasPerms2));
        }); 
        importBuildingPos = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.openFile(ProjectManagementWindow.instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "Building Positions",
                        Extensions = new List<string>() {
                            "bld",
                            "bin"
                        }
                    }
                });
                currentMapContainer.buildingPositions = File.ReadAllBytes(path);
                this.RaisePropertyChanged(nameof(hasBldPos));
            }
            catch (OperationCanceledException ex) {
            }
        });
        exportBuildingPos = ReactiveCommand.Create(async () => {
            try {
                string path = await UI.saveFile(ProjectManagementWindow.instance, new List<FileDialogFilter>() {
                    new() {
                        Name = "Building Positions",
                        Extensions = new List<string>() {
                            "bld",
                            "bin"
                        }
                    }
                });
                File.WriteAllBytes(path, currentMapContainer.buildingPositions);
            }
            catch (OperationCanceledException ex) {
            }
        });  
        removeBuildingPos = ReactiveCommand.Create(async () => {
            currentMapContainer.buildingPositions = Array.Empty<byte>();
            this.RaisePropertyChanged(nameof(hasBldPos));
        }); 
        selectedIndex = 0;
    }
    
    public override void onAddNew() {
        throw new System.NotImplementedException();
    }

    public override void onRemoveSelected(int index) {
        throw new System.NotImplementedException();
    }

    public override void onIndexChange(int newValue) {
        if (newValue >= 0 && newValue < UI.patcher.getNarcEntryCount(UI.gameInfo.maps)) {
            this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
            this.RaisePropertyChanged(nameof(selectedIndex));
            currentMapContainer = new MapContainer(UI.patcher.fetchFileFromNarc(UI.gameInfo.maps, selectedIndex));
        }
    }

    public override void onSaveChanges() {
        UI.patcher.saveToNarcFolder(UI.gameInfo.maps, selectedIndex, x => currentMapContainer.serialize(x));
    }
}