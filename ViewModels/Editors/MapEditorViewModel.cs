using System;
using System.Collections.Generic;
using Avalonia.Controls;
using BeaterLibrary.Formats.Maps;
using ReactiveUI;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.Models.Editors;
using SwissArmyKnife.ViewModels.Base;

namespace SwissArmyKnife.ViewModels.Editors; 

public class MapEditorViewModel : EditorViewModelBase {
    private Dictionary<int, MapEditorModel> LoadedMapContainers;
    private MapEditorModel Current;
    public bool AnyContainers => LoadedMapContainers.Count > 0;
    
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
        }
    }

    public override void OnSaveChanges() {
        throw new NotImplementedException();
    }

    protected override void TryShowTabControl() {
        this.RaisePropertyChanged(nameof(AnyContainers));
        if (AnyContainers) {
            Current = LoadedMapContainers[CurrentTab];
        }
    }

    private MapContainer GetMapContainerFromARC(int ID) {
        return new MapContainer(GameWork.Patcher.GetARCFile(ARC, ID));
    }
}