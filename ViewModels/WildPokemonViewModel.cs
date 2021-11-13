using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using BeaterLibrary.Formats.Pokémon;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.ViewModels
{
    public class WildPokemonViewModel : ViewModelBase
    {
        private bool _EnableAddButton;
        private bool _EnableRemoveButton;
        private int _SelectedEntry;

        private int _SelectedTab;

        private readonly Dictionary<int, ObservableCollection<WildEncounterEntry>> Map;
        private int MaximumEntries;

        public WildPokemonViewModel()
        {
            GrassEntries = new ObservableCollection<WildEncounterEntry>();
            GrassDoubleEntries = new ObservableCollection<WildEncounterEntry>();
            GrassShakeEntries = new ObservableCollection<WildEncounterEntry>();
            SurfEntries = new ObservableCollection<WildEncounterEntry>();
            SurfSpotEntries = new ObservableCollection<WildEncounterEntry>();
            FishEntries = new ObservableCollection<WildEncounterEntry>();
            FishSpotEntries = new ObservableCollection<WildEncounterEntry>();
            Map = new Dictionary<int, ObservableCollection<WildEncounterEntry>>
            {
                [0] = GrassEntries,
                [1] = GrassDoubleEntries,
                [2] = GrassShakeEntries,
                [3] = SurfEntries,
                [4] = SurfSpotEntries,
                [5] = FishEntries,
                [6] = FishSpotEntries
            };
            AddNew = ReactiveCommand.Create(() =>
            {
                CurrentCollection.Add(new WildEncounterEntry());
                SelectedEntry = CurrentCollection.Count - 1;
                EnableAddButton = CurrentCollection.Count < MaximumEntries;
                EnableRemoveButton = CurrentCollection.Count > 0;
            });
            RemoveSelected = ReactiveCommand.Create(() =>
            {
                CurrentCollection.RemoveAt(SelectedEntry);
                if (CurrentCollection.Count > 0)
                    SelectedEntry = 0;
                EnableAddButton = CurrentCollection.Count < MaximumEntries;
                EnableRemoveButton = CurrentCollection.Count > 0;
            });
            SelectedTab = 0;
            SelectedEntry = -1;
        }

        public WildPokemonViewModel(WildEncounters WildEncounterEntries) : this()
        {
            GrassEntries = new ObservableCollection<WildEncounterEntry>(WildEncounterEntries.GrassEntries);
            GrassDoubleEntries = new ObservableCollection<WildEncounterEntry>(WildEncounterEntries.GrassDoubleEntries);
            GrassShakeEntries = new ObservableCollection<WildEncounterEntry>(WildEncounterEntries.GrassShakeEntries);
            SurfEntries = new ObservableCollection<WildEncounterEntry>(WildEncounterEntries.SurfEntries);
            SurfSpotEntries = new ObservableCollection<WildEncounterEntry>(WildEncounterEntries.SurfSpotEntries);
            FishEntries = new ObservableCollection<WildEncounterEntry>(WildEncounterEntries.FishEntries);
            FishSpotEntries = new ObservableCollection<WildEncounterEntry>(WildEncounterEntries.FishSpotEntries);
            Map[0] = GrassEntries;
            Map[1] = GrassDoubleEntries;
            Map[2] = GrassShakeEntries;
            Map[3] = SurfEntries;
            Map[4] = SurfSpotEntries;
            Map[5] = FishEntries;
            Map[6] = FishSpotEntries;
            SelectedTab = 0;
            SelectedEntry = -1;
        }

        public WildEncounterEntry CurrentWildEntry { get; private set; }
        public ObservableCollection<WildEncounterEntry> CurrentCollection { get; private set; }
        public ObservableCollection<WildEncounterEntry> GrassEntries { get; }
        public ObservableCollection<WildEncounterEntry> GrassDoubleEntries { get; }
        public ObservableCollection<WildEncounterEntry> GrassShakeEntries { get; }
        public ObservableCollection<WildEncounterEntry> SurfEntries { get; }
        public ObservableCollection<WildEncounterEntry> SurfSpotEntries { get; }
        public ObservableCollection<WildEncounterEntry> FishEntries { get; }
        public ObservableCollection<WildEncounterEntry> FishSpotEntries { get; }
        public ReactiveCommand<Unit, Unit>? AddNew { get; }
        public ReactiveCommand<Unit, Unit>? RemoveSelected { get; }

        public bool EnableAddButton
        {
            get => _EnableAddButton;
            set => this.RaiseAndSetIfChanged(ref _EnableAddButton, value);
        }

        public bool EnableRemoveButton
        {
            get => _EnableRemoveButton;
            set => this.RaiseAndSetIfChanged(ref _EnableRemoveButton, value);
        }

        public int SelectedEntry
        {
            get => _SelectedEntry;
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedEntry, value);
                if (value != -1)
                {
                    CurrentWildEntry = Map[SelectedTab][SelectedEntry];
                    this.RaisePropertyChanged("CurrentWildEntry");
                }
            }
        }

        public int SelectedTab
        {
            get => _SelectedTab;
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedTab, value);
                CurrentCollection = Map[SelectedTab];
                MaximumEntries = SelectedTab > 3 ? 0x5 : 0xC;
                EnableAddButton = CurrentCollection.Count < MaximumEntries;
                EnableRemoveButton = CurrentCollection.Count > 0;
                this.RaisePropertyChanged("CurrentCollection");
            }
        }
    }
}