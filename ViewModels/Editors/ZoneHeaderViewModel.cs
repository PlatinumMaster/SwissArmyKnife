using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using BeaterLibrary.Formats.Maps;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.GameInfo;
using DynamicData.Kernel;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public class ZoneHeaderViewModel : ViewModelTemplate {
        private int _selectedIndex, _selectedNameIndex;
        private List<MapHeader> mapHeaders { get; set; }
        public ObservableCollection<string> mapHeaderNames { get; }
        public ObservableCollection<string> mapNames { get; }
        private MapHeader currentHeader => mapHeaders[selectedIndex];
        public ReactiveCommand<Unit, Unit> loadZone { get; }

        public override int selectedIndex {
            get => _selectedIndex;
            set => onIndexChange(value);
        }
        
        public int selectedNameIndex {
            get => currentHeader.nameIndex;
            set {
                currentHeader.nameIndex = (ushort) value;
                mapHeaderNames[selectedIndex] = $"{selectedIndex} - {mapNames[currentHeader.nameIndex]}";
                this.RaisePropertyChanged(nameof(mapHeaderNames));
            }
        }

        public ZoneHeaderViewModel() {
            loadZone = ReactiveCommand.Create(() => {
                this.RaisePropertyChanged(nameof(currentHeader));
                this.RaisePropertyChanged(nameof(selectedNameIndex));
            });
            var data = UI.patcher.fetchFileFromNarc(UI.gameInfo.zoneHeaders, 0);
            mapHeaders = new MapHeaders(data).headers;
            mapNames = new ObservableCollection<string>(
                new TextContainer(UI.patcher.fetchFileFromNarc(UI.gameInfo.systemsText,
                    UI.gameInfo.ImportantSystemText["MapNames"])).fetchTextAsStringArray());
            mapHeaderNames = new ObservableCollection<string>();
            for (int i = 0; i < mapHeaders.Count; ++i) {
                mapHeaderNames.Add($"{i} - {mapNames[mapHeaders[i].nameIndex]}");
            }
        }

        public override void onAddNew() {
            mapHeaders.Add(new MapHeader(mapHeaders.Count));
        }

        public override void onRemoveSelected(int index) {
            if (index < mapHeaders.Count && index >= 0) 
                mapHeaders.RemoveAt(index);
        }

        public override void onIndexChange(int newValue) {
            if (newValue < mapHeaders.Count && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
                this.RaisePropertyChanged(nameof(selectedIndex));
            }
        }

        public override void onSaveChanges() {
            UI.patcher.saveToNarcFolder(UI.gameInfo.zoneHeaders, 0, x => MapHeaders.serialize(mapHeaders.AsList(), x));
        }
    }
}