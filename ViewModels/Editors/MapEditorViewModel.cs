using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using AvaloniaEdit.Document;
using BeaterLibrary.Formats.Maps;
using DynamicData;
using ReactiveUI;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.Models.Editors;
using SwissArmyKnife.ViewModels.Base;

namespace SwissArmyKnife.ViewModels.Editors; 

public class MapEditorViewModel : EditorViewModelBase {
    private Dictionary<int, MapEditorModel> LoadedMapContainers;
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
        }
    }

    private MapContainer GetMapContainerFromARC(int ID) {
        return new MapContainer(GameWork.Patcher.GetARCFile(ARC, ID));
    }
}  