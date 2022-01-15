using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using BeaterLibrary.Formats.Maps;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors; 

public class MatrixEditorViewModel : ViewModelTemplate {
    private int _selectedIndex;
    private MapMatrix currentMatrix;
    public ObservableCollection<MapMatrixRow> currentMapFileMatrix { get; set; }
    public ObservableCollection<MapMatrixRow> currentMapHeaderDataTable { get; set; }

    public override int selectedIndex {
        get => _selectedIndex;
        set => onIndexChange(value);
    }

    public MatrixEditorViewModel() {
        selectedIndex = 0;
    }
    
    public override void onAddNew() {
        throw new System.NotImplementedException();
    }

    public override void onRemoveSelected(int index) {
        throw new System.NotImplementedException();
    }

    public override void onIndexChange(int newValue) {
        if (newValue >= 0 && newValue < UI.patcher.getNarcEntryCount(UI.gameInfo.matrix)) {
            this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
            currentMatrix = new MapMatrix(UI.patcher.fetchFileFromNarc(UI.gameInfo.matrix, newValue));
            currentMapFileMatrix = new ObservableCollection<MapMatrixRow>(currentMatrix.mapFilesMatrix);
            this.RaisePropertyChanged(nameof(currentMapFileMatrix));
        }
    }

    public override void onSaveChanges() {
        throw new System.NotImplementedException();
    }
}