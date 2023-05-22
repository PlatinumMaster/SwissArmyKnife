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
        private List<MapHeader> MapHeaders { get; set; }
        public ObservableCollection<string> MapHeaderNames { get; }
        public ObservableCollection<string> MapNames { get; }
        private MapHeader CurrentHeader => MapHeaders[SelectedIndex];
        public ReactiveCommand<Unit, Unit> LoadZone { get; }

        public override int SelectedIndex {
            get => _selectedIndex;
            set => OnIndexChange(value);
        }
        
        public int SelectedNameIndex {
            get => CurrentHeader.nameIndex;
            set {
                CurrentHeader.nameIndex = (ushort) value;
                MapHeaderNames[SelectedIndex] = $"{SelectedIndex} - {MapNames[CurrentHeader.nameIndex]}";
                this.RaisePropertyChanged(nameof(MapHeaderNames));
            }
        }

        public ZoneHeaderViewModel() {
            LoadZone = ReactiveCommand.Create(() => {
                this.RaisePropertyChanged(nameof(CurrentHeader));
                this.RaisePropertyChanged(nameof(SelectedNameIndex));
            });
            var data = UI.Patcher.fetchFileFromNarc(UI.GameInfo.zoneHeaders, 0);
            MapHeaders = new MapHeaders(data).headers;
            MapNames = new ObservableCollection<string>(
                new TextContainer(UI.Patcher.fetchFileFromNarc(UI.GameInfo.systemsText,
                    UI.GameInfo.ImportantSystemText["MapNames"])).fetchTextAsStringArray());
            MapHeaderNames = new ObservableCollection<string>();
            for (int i = 0; i < MapHeaders.Count; ++i) {
                MapHeaderNames.Add($"{i} - {MapNames[MapHeaders[i].nameIndex]}");
            }
        }

        public override void OnAddNew() {
            MapHeaders.Add(new MapHeader(MapHeaders.Count));
        }

        public override void OnRemoveSelected(int index) {
            if (index < MapHeaders.Count && index >= 0) 
                MapHeaders.RemoveAt(index);
        }

        public override void OnIndexChange(int newValue) {
            if (newValue < MapHeaders.Count && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
                this.RaisePropertyChanged(nameof(SelectedIndex));
            }
        }

        public override void OnSaveChanges() {
            UI.Patcher.saveToNarcFolder(UI.GameInfo.zoneHeaders, 0, x => BeaterLibrary.Formats.Maps.MapHeaders.serialize(MapHeaders.AsList(), x));
        }
    }
}