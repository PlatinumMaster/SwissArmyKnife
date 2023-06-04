using System;
using BeaterLibrary.Formats.Maps;
using BeaterLibrary.Formats.Nitro;
using ReactiveUI;

namespace SwissArmyKnife.Models.Editors; 

public class MapEditorModel : ReactiveObject {
    private MapContainer _Container;
    public MapContainer Container {
        get => _Container;
        set {
            this.RaiseAndSetIfChanged(ref _Container, value);
            UpdateContainerType();
            this.RaisePropertyChanged(nameof(ContainerType));
            this.RaisePropertyChanged(nameof(MapModel));
            this.RaisePropertyChanged(nameof(Permissions));
            this.RaisePropertyChanged(nameof(Permissions2));
            this.RaisePropertyChanged(nameof(BuildingPositions));
        }
    }

    public ushort ContainerType {
        get => (ushort) Array.IndexOf(MapContainer.MapContainerType.GetValues(Container.ContainerType.GetType()), Container.ContainerType);
    }

    public NSBMD MapModel {
        get => Container.Model;
        set {
            Container.Model = value;
        }
    } 
    
    public byte[] Permissions {
        get => Container.Permissions;
        set {
            Container.Permissions = value;
            UpdateContainerType();
        }
    }

    public bool HasPermissions => Permissions != null && Permissions.Length > 0;

    public byte[] Permissions2 {
        get => Container.Permissions2;
        set {
            Container.Permissions2 = value;
            UpdateContainerType();
        }
    } 
    
    public bool HasPermissions2 => Permissions2 != null && Permissions2.Length > 0;

    public byte[] BuildingPositions {
        get => Container.BuildingPositions;
        set {
            Container.BuildingPositions = value;
        }
    }

    public bool HasBuildingPositions => BuildingPositions != null && BuildingPositions.Length > 0;

    public void UpdateContainerType() {
        // Assume model & building positions are set.
        if (Permissions != null && Permissions2 == null) {
            // WB or RD chunk. My tool does not support RD natively.
            Container.ContainerType = MapContainer.MapContainerType.WB;
        } else if (Permissions != null && Permissions2 != null) {
            // GC Chunk
            Container.ContainerType = MapContainer.MapContainerType.GC;
        } else {
            // NG Chunk
            Container.ContainerType = MapContainer.MapContainerType.NG;
        }
    }

    public int Index { get; set; }
}