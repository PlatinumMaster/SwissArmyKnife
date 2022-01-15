using System.Collections.ObjectModel;
using BeaterLibrary.Formats.Maps;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.GameInfo;
using DynamicData.Kernel;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public class HeaderViewModel : ViewModelTemplate {
        private int _selectedIndex;
        public ObservableCollection<MapHeader> mapHeaders { get; set; }
        public ObservableCollection<string> mapNames { get; }
        private MapHeader currentHeader => mapHeaders[selectedIndex];
        public HeaderViewModel() {
            var data = UI.patcher.fetchFileFromNarc(UI.gameInfo.zoneHeaders, 0);
            mapHeaders = new ObservableCollection<MapHeader>(new MapHeaders(data).headers);
            mapNames = new ObservableCollection<string>(
                new TextContainer(UI.patcher.fetchFileFromNarc(UI.gameInfo.systemsText,
                    (int) B2W2.ImportantSystemText.MapNames)).fetchTextAsStringArray());
        }

        public override int selectedIndex {
            get => _selectedIndex;
            set => onIndexChange(value);
        }

        public override void onAddNew() {
            mapHeaders.Add(new MapHeader(mapHeaders.Count));
        }

        public override void onRemoveSelected(int index) {
            if (index < mapHeaders.Count && index >= 0) mapHeaders.RemoveAt(index);
        }

        public override void onIndexChange(int newValue) {
            if (newValue < mapHeaders.Count && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
                this.RaisePropertyChanged("currentHeader");
            }
        }

        public override void onSaveChanges() {
            UI.patcher.saveToNarcFolder(UI.gameInfo.zoneHeaders, 0, x => MapHeaders.serialize(mapHeaders.AsList(), x));
        }
    }
}