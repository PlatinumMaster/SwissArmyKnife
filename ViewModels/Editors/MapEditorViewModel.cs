using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaEdit.Document;
using BeaterLibrary.Formats.Maps;
using BeaterLibrary.Formats.Nitro;
using DynamicData;
using ReactiveUI;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.Models.Editors;
using SwissArmyKnife.ViewModels.Base;

namespace SwissArmyKnife.ViewModels.Editors; 

public class MapEditorViewModel : EditorViewModelBase {
    private Dictionary<int, MapEditorModel> LoadedMapContainers;
    public ReactiveCommand<Unit, Unit> ImportModel { get; }
    public ReactiveCommand<Unit, Unit> ExportModel { get; }
    public ReactiveCommand<Unit, Unit> ImportPermissions { get; }
    public ReactiveCommand<Unit, Unit> ExportPermissions { get; }
    public ReactiveCommand<Unit, Unit> RemovePermissions { get; }
    public ReactiveCommand<Unit, Unit> ImportPermissions2 { get; }
    public ReactiveCommand<Unit, Unit> ExportPermissions2 { get; }
    public ReactiveCommand<Unit, Unit> RemovePermissions2 { get; }
    public ReactiveCommand<Unit, Unit> ImportBuildingPos { get; }
    public ReactiveCommand<Unit, Unit> ExportBuildingPos { get; }
    public ReactiveCommand<Unit, Unit> RemoveBuildingPos { get; }
    public MapEditorModel Current { get; private set; }
    public bool AnyContainers => LoadedMapContainers.Count > 0;

    public MapEditorViewModel() {
        ARC = GameWork.Project.GameInfo.ARCs["Map Containers"];
        Max = GameWork.Patcher.GetARCMax(ARC);
        Current = new MapEditorModel {
            Container = new MapContainer(MapContainer.MapContainerType.NG)
        };
        Tabs = new ObservableCollection<TabItem>();
        LoadedMapContainers = new Dictionary<int, MapEditorModel>();
        ImportModel = ReactiveCommand.Create(ImportModelFromDisk);
        ExportModel = ReactiveCommand.Create(ExportModelToDisk);
        ImportPermissions = ReactiveCommand.Create(ImportPermissionsFromDisk);
        ExportPermissions = ReactiveCommand.Create(ExportPermissionsToDisk);
        RemovePermissions = ReactiveCommand.Create(RemovePermissionsFromContainer);
        ImportPermissions2 = ReactiveCommand.Create(ImportPermissions2FromDisk);
        ExportPermissions2 = ReactiveCommand.Create(ExportPermissions2ToDisk);
        RemovePermissions2 = ReactiveCommand.Create(RemovePermissions2FromContainer);
        ImportBuildingPos = ReactiveCommand.Create(ImportBuildingPositionsFromDisk);
        ExportBuildingPos = ReactiveCommand.Create(ExportBuildingPositionsToDisk);
        RemoveBuildingPos = ReactiveCommand.Create(RemoveBuildingPositionsFromContainer);
    }
    
    public override void OnAddNew() {
        throw new NotImplementedException();
    }

    public override void OnRemoveSelected() {
        throw new NotImplementedException();
    }

    public async override void OnLoadFile() {
        if (GameWork.Patcher != null && SelectedIndex >= 0 && SelectedIndex <= Max) {
            if (LoadedMapContainers.ContainsKey(SelectedIndex)) {
                // File already loaded, prompt.
                if (!await RefreshPromptConfirm("Reload Map Container", "This map container is already open. Would you like to reload it anyway? All unsaved changes will be lost.")) {
                    return;
                }
            }

            MapContainer LoadedContainer = GetMapContainerFromARC(SelectedIndex);
            LoadedMapContainers.Add(SelectedIndex, new MapEditorModel {
                Container = LoadedContainer,
                Index = SelectedIndex
            });
            
            Tabs.Add(new TabItem {
                Header = $"Map Container {SelectedIndex}",
            });

            CurrentTab = LoadedMapContainers.Keys.IndexOf(SelectedIndex);
        }
    }

    public override void OnSaveChanges() {
        throw new NotImplementedException();
    }

    protected override void TryShowTabControl() {
        this.RaisePropertyChanged(nameof(AnyContainers));
        if (AnyContainers) {
            Current = LoadedMapContainers[SelectedIndex];
            this.RaisePropertyChanged(nameof(Current));
            this.RaisePropertyChanged(nameof(Current.ModelName));
            this.RaisePropertyChanged(nameof(Current.HasPermissions));
            this.RaisePropertyChanged(nameof(Current.HasPermissions2));
            this.RaisePropertyChanged(nameof(Current.HasBuildingPositions));
        }
    }

    private MapContainer GetMapContainerFromARC(int ID) {
        return new MapContainer(GameWork.Patcher.GetARCFile(ARC, ID));
    }

    private async void ImportModelFromDisk() {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            string? FilePath = await IO.OpenFile(Desktop.MainWindow, new List<FileDialogFilter>() {
            });
            if (FilePath != null && !FilePath.Equals("")) {
                try {
                    Current.MapModel = new NSBMD(File.ReadAllBytes(FilePath));
                }
                catch (Exception e) {
                    Messages.Error(Desktop, "Error when importing model", $"An error occurred when importing your model, and the process has been aborted.\nThe error is as follows:\n{e}");
                }
                this.RaisePropertyChanged(nameof(Current.ModelName));
            }
        }
    }
    
    private async void ExportModelToDisk() {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            string? FilePath = await IO.SaveFile(Desktop.MainWindow, new List<FileDialogFilter>() {
                new FileDialogFilter() {
                    
                }
            });
            if (FilePath != null) {
                // TODO
            }
        }
    }
    
    private async void ImportPermissionsFromDisk() {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            string? FilePath = await IO.OpenFile(Desktop.MainWindow, new List<FileDialogFilter>() {
            });
            if (FilePath != null) {
                // TODO
                this.RaisePropertyChanged(nameof(Current.HasPermissions));
            }
        }
    }
    
    private async void ExportPermissionsToDisk() {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            string? FilePath = await IO.OpenFile(Desktop.MainWindow, new List<FileDialogFilter>() {
            });
            if (FilePath != null) {
                // TODO
            }
        }
    }
    
    private async void RemovePermissionsFromContainer() {
        Current.Permissions = Array.Empty<byte>();
        this.RaisePropertyChanged(nameof(Current.HasPermissions));
    }
    
    private async void ImportPermissions2FromDisk() {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            string? FilePath = await IO.OpenFile(Desktop.MainWindow, new List<FileDialogFilter>() {
            });
            if (FilePath != null) {
                // TODO
                this.RaisePropertyChanged(nameof(Current.HasPermissions2));
            }
        }
    }
    
    private async void ExportPermissions2ToDisk() {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            string? FilePath = await IO.OpenFile(Desktop.MainWindow, new List<FileDialogFilter>() {
            });
            if (FilePath != null) {
                // TODO
            }
        }
    }
    
    private async void RemovePermissions2FromContainer() {
        Current.Permissions2 = Array.Empty<byte>();
        this.RaisePropertyChanged(nameof(Current.HasPermissions2));
    }
    
    private async void ImportBuildingPositionsFromDisk() {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            string? FilePath = await IO.OpenFile(Desktop.MainWindow, new List<FileDialogFilter>() {
            });
            if (FilePath != null) {
                // TODO
                this.RaisePropertyChanged(nameof(Current.HasBuildingPositions));
            }
        }
    }
    
    private async void ExportBuildingPositionsToDisk() {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            string? FilePath = await IO.OpenFile(Desktop.MainWindow, new List<FileDialogFilter>() {
            });
            if (FilePath != null) {
                // TODO
            }
        }
    }
    
    private async void RemoveBuildingPositionsFromContainer() {
        Current.BuildingPositions = Array.Empty<byte>();
        this.RaisePropertyChanged(nameof(Current));
    }
}  