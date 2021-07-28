using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using BeaterLibrary.Formats.Maps;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.Controls.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        public ObservableCollection<MapHeader> MapHeaders { get; }
        public MapHeader CurrentMapHeader { get; private set; }
        public ReactiveCommand<Unit, Unit> AddNew { get; }
        public ReactiveCommand<Unit, Unit> RemoveSelected { get; }

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

        private int _SelectedZone;

        public HeaderViewModel()
        {
            MapHeaders = new ObservableCollection<MapHeader> { new MapHeader(0) };
            AddNew = ReactiveCommand.Create(() => { MapHeaders.Add(new MapHeader(MapHeaders.Count)); });
            RemoveSelected = ReactiveCommand.Create(() =>
            {
                if (SelectedZone != -1 && MapHeaders.Count > 1)
                {
                    MapHeaders.RemoveAt(SelectedZone);
                    for (int i = 0; i < MapHeaders.Count; ++i)
                        MapHeaders[i].Index = i;
                }
            });
            SelectedZone = 0;
        }

        public HeaderViewModel(List<MapHeader> MapHeaders) : this()
        {
            this.MapHeaders = new ObservableCollection<MapHeader>(MapHeaders);
        }
    }
}