using BeaterLibrary.Formats.Maps;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.Models.Editors; 

public class MapEditorModel : ReactiveObject {
    private MapContainer _Container;
    public MapContainer Container {
        get => _Container;
        set => this.RaiseAndSetIfChanged(ref _Container, value);
    }
    public int Index { get; set; }
}