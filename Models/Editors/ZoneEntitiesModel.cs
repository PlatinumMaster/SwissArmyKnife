using BeaterLibrary.Formats.Zone;
using ReactiveUI;

namespace SwissArmyKnife.Models.Editors; 

public class ZoneEntitiesModel : ReactiveObject {
    private ZoneEntities _Container;
    public ZoneEntities Container {
        get => _Container;
        set => this.RaiseAndSetIfChanged(ref _Container, value);
    }
    public int Index { get; set; }
}