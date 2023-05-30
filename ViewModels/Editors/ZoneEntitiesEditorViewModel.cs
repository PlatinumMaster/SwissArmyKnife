using SwissArmyKnife.Avalonia.ViewModels.Base;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors; 

public class ZoneEntitiesEditorViewModel : EditorViewModelBase {
    // private List<ZoneEntitiesModel> Models;
    // public ZoneEntitiesModel Current { get; private set; }
    // public bool IsZoneEntitiesEditorVisible => Models.Count > 0;
    //
    // public ZoneEntitiesEditorViewModel() {
    //     Models = new List<ZoneEntitiesModel>();
    //     FSPath = GameController.CurrentGameData.ZoneEntities;
    //     if (GameController.PatcherInstance != null) {
    //         Max = GameController.PatcherInstance.GetMaximumNARCEntryIndex(FSPath);
    //     }
    // }
    //
    // public override void OnAddNew() {
    //     throw new System.NotImplementedException();
    // }
    //
    // public override void OnRemoveSelected() {
    //     throw new System.NotImplementedException();
    // }
    //
    // public override async void OnLoadFile() {
    //     if (GameController.PatcherInstance != null && SelectedIndex >= 0 && SelectedIndex <= Max) {
    //         bool IsOpen = Models.Any(x => x.Index == SelectedIndex);
    //         if (IsOpen) {
    //             if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
    //                 DialogResult Result = await Messages.YesNoMessage(Desktop, "Reload Zone Entities Container", "This container is already open. Would you like to reload it anyway? All unsaved changes will be lost.");
    //                 if (Result.GetResult.Equals("No")) {
    //                     return;
    //                 }
    //             }
    //         }
    //
    //         ZoneEntities Container =
    //             new ZoneEntities(
    //                 GameController.PatcherInstance.FetchFileFromNARC(GameController.CurrentGameData.ZoneEntities,
    //                     SelectedIndex));
    //         
    //         ZoneEntitiesModel Model;
    //         if (IsOpen) {
    //             Model = Models.Find(x => x.Index.Equals(SelectedIndex));
    //             Model.Container = Container;
    //         } else {
    //             Models.Add(new ZoneEntitiesModel() {
    //                 Container = Container,
    //                 Index = SelectedIndex
    //             });
    //             Tabs.Add(new TabItem {
    //                 Header = $"Zone Entities Container {SelectedIndex}"
    //             });
    //         }
    //         
    //         CurrentTab = Models.FindIndex(x => x.Index == SelectedIndex);
    //     }
    // }
    //
    // public override void OnSaveChanges() {
    //     throw new System.NotImplementedException();
    // }
    //
    // protected override void TryShowTabControl() {
    //     this.RaisePropertyChanged(nameof(IsZoneEntitiesEditorVisible));
    //     if (IsZoneEntitiesEditorVisible) {
    //         Current = Models[CurrentTab];
    //         this.RaisePropertyChanged(nameof(Current));
    //     }
    // }
    public override void OnAddNew() {
        throw new System.NotImplementedException();
    }

    public override void OnRemoveSelected() {
        throw new System.NotImplementedException();
    }

    public override void OnLoadFile() {
        throw new System.NotImplementedException();
    }

    public override void OnSaveChanges() {
        throw new System.NotImplementedException();
    }

    protected override void TryShowTabControl() {
        throw new System.NotImplementedException();
    }
}