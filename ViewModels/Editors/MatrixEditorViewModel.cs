using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using BeaterLibrary.Formats.Maps;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors; 

public class MatrixEditorViewModel : ViewModelTemplate {
    private int _selectedIndex;
    private MapMatrix _currentMatrix;
    public ObservableCollection<MapMatrixRow> CurrentMapFileMatrix { get; set; }
    public ObservableCollection<MapMatrixRow> CurrentMapHeaderDataTable { get; set; }

    public override int SelectedIndex {
        get => _selectedIndex;
        set => OnIndexChange(value);
    }

    public MatrixEditorViewModel() {
        SelectedIndex = 0;
    }
    
    public override void OnAddNew() {
        throw new System.NotImplementedException();
    }

    public override void OnRemoveSelected(int index) {
        throw new System.NotImplementedException();
    }

    public override void OnIndexChange(int newValue) {
        if (newValue >= 0 && newValue < UI.Patcher.getNarcEntryCount(UI.GameInfo.matrix)) {
            this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
            _currentMatrix = new MapMatrix(UI.Patcher.fetchFileFromNarc(UI.GameInfo.matrix, newValue));
            CurrentMapFileMatrix = new ObservableCollection<MapMatrixRow>(_currentMatrix.mapFilesMatrix);
            this.RaisePropertyChanged(nameof(CurrentMapFileMatrix));
        }
    }

    public override void OnSaveChanges() {
        throw new System.NotImplementedException();
    }
}