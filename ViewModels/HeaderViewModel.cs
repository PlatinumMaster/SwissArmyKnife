using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using BeaterLibrary.Formats.Maps;
using DynamicData.Kernel;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        private int _SelectedZone;

        public HeaderViewModel()
        {
            MapHeaders = new ObservableCollection<MapHeader> {new(0)};
            AddNew = ReactiveCommand.Create(() => { MapHeaders.Add(new MapHeader(MapHeaders.Count)); });
            RemoveSelected = ReactiveCommand.Create(() =>
            {
                if (SelectedZone != -1 && MapHeaders.Count > 1)
                {
                    MapHeaders.RemoveAt(SelectedZone);
                    for (var i = 0; i < MapHeaders.Count; ++i)
                        MapHeaders[i].Index = i;
                }
            });
            SaveFile = ReactiveCommand.Create(() =>
            {
                UIUtil.BaseROMPatcher.SaveToNARCFolder(UIUtil.CurrentGameInformation.ZoneHeader, 0,
                    x => BeaterLibrary.Formats.Maps.MapHeaders.Serialize(MapHeaders.AsList(), x));
            });
            SelectedZone = 0;
        }

        public HeaderViewModel(List<MapHeader> MapHeaders) : this()
        {
            this.MapHeaders = new ObservableCollection<MapHeader>(MapHeaders);
        }

        public ObservableCollection<MapHeader> MapHeaders { get; }
        public MapHeader CurrentMapHeader { get; private set; }
        public ReactiveCommand<Unit, Unit> AddNew { get; }
        public ReactiveCommand<Unit, Unit> RemoveSelected { get; }
        public ReactiveCommand<Unit, Unit> SaveFile { get; set; }

        public int SelectedZone
        {
            get => _SelectedZone;
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedZone, value);
                if (SelectedZone != -1)
                    CurrentMapHeader = MapHeaders[SelectedZone];
                this.RaisePropertyChanged("CurrentMapHeader");
            }
        }
    }
}