using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using BeaterLibrary.Formats.Pokémon;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.GameInfo;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public class WildPokemonViewModel : ViewModelTemplate {
        private int _selectedIndex;
        private int _selectedSubEntry;
        private int _numberOfEntries;

        private List<ObservableCollection<WildEncounterEntry>> _containerGrassEntries, _containerGrassDoubleEntries, _containerGrassSpecialEntries;
        private List<ObservableCollection<WildEncounterEntry>> _containerSurfEntries, _containerSurfSpecialEntries;
        private List<ObservableCollection<WildEncounterEntry>> _containerFishEntries, _containerFishSpecialEntries;

        public ObservableCollection<WildEncounterEntry> GrassSingleEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> GrassDoubleEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> GrassSpecialEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> SurfEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> SurfSpecialEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> FishEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> FishSpecialEntries { get; set; }
        public ObservableCollection<string> SpeciesNames { get; set; }
        public ReactiveCommand<Unit, Unit> LoadContainer { get; }
        public ReactiveCommand<Unit, Unit> LoadSubentry { get; }

        public WildPokemonViewModel() {
            SpeciesNames = new ObservableCollection<string>(
                new TextContainer(UI.Patcher.fetchFileFromNarc(UI.GameInfo.systemsText,
                    UI.GameInfo.ImportantSystemText["PokémonNames"])).fetchTextAsStringArray());
            this.RaisePropertyChanged(nameof(SpeciesNames));
            LoadContainer = ReactiveCommand.Create(() => ChangeWildContainer(SelectedIndex));
            LoadSubentry = ReactiveCommand.Create(() => OnSelectedSubentryChange(SelectedSubentryIndex));
            _containerGrassEntries = new List<ObservableCollection<WildEncounterEntry>>();
            _containerGrassDoubleEntries = new List<ObservableCollection<WildEncounterEntry>>();
            _containerGrassSpecialEntries = new List<ObservableCollection<WildEncounterEntry>>();
            _containerFishEntries = new List<ObservableCollection<WildEncounterEntry>>();
            _containerFishSpecialEntries = new List<ObservableCollection<WildEncounterEntry>>();
            _containerSurfEntries = new List<ObservableCollection<WildEncounterEntry>>();
            _containerSurfSpecialEntries = new List<ObservableCollection<WildEncounterEntry>>();
        }
        
        public override int SelectedIndex {
            get => _selectedIndex;
            set => OnIndexChange(value);
        }
        
        public int SelectedSubentryIndex {
            get => _selectedSubEntry;
            set => OnSelectedSubentryChange(value);
        }

        public override void OnIndexChange(int newValue) {
            if (newValue >= 0 && newValue < UI.Patcher.getNarcEntryCount(UI.GameInfo.wildEncounters)) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
            }
        }

        private void OnSelectedSubentryChange(int newValue) {
            if (newValue >= 0 && newValue < _numberOfEntries) {
                this.RaiseAndSetIfChanged(ref _selectedSubEntry, newValue);
            }
            GrassSingleEntries = _containerGrassEntries[SelectedSubentryIndex];
            GrassDoubleEntries = _containerGrassDoubleEntries[SelectedSubentryIndex];
            GrassSpecialEntries = _containerGrassSpecialEntries[SelectedSubentryIndex];
            SurfEntries = _containerSurfEntries[SelectedSubentryIndex];
            SurfSpecialEntries = _containerSurfSpecialEntries[SelectedSubentryIndex];
            FishEntries = _containerFishEntries[SelectedSubentryIndex];
            FishSpecialEntries = _containerFishSpecialEntries[SelectedSubentryIndex];
            
            this.RaisePropertyChanged(nameof(GrassSingleEntries));
            this.RaisePropertyChanged(nameof(GrassDoubleEntries));
            this.RaisePropertyChanged(nameof(GrassSpecialEntries));
            this.RaisePropertyChanged(nameof(SurfEntries));
            this.RaisePropertyChanged(nameof(SurfSpecialEntries));
            this.RaisePropertyChanged(nameof(FishEntries));
            this.RaisePropertyChanged(nameof(FishSpecialEntries));
        }

        private void ResetContainerEntries() {
            _containerGrassEntries.Clear();
            _containerGrassDoubleEntries.Clear();
            _containerGrassSpecialEntries.Clear();
            _containerFishEntries.Clear();
            _containerFishSpecialEntries.Clear();
            _containerSurfEntries.Clear();
            _containerSurfSpecialEntries.Clear();
        }

        private void ChangeWildContainer(int Index) {
            ResetContainerEntries();
            try {
                byte[] buffer = UI.Patcher.fetchFileFromNarc(UI.GameInfo.wildEncounters, Index);
                if (buffer.Length > 0 && buffer.Length % 0xE8 == 0) {
                    _numberOfEntries = buffer.Length / 0xE8;
                    for (int index = 0; index < _numberOfEntries; ++index) {
                        WildEncounters e = new(buffer.Skip(0xE8 * index).Take(0xE8).ToArray());
                        _containerGrassEntries.Add(new(e.grassEntries));
                        _containerGrassDoubleEntries.Add(new(e.grassDoubleEntries));
                        _containerGrassSpecialEntries.Add(new(e.grassSpecialEntries));
                        _containerSurfEntries.Add(new(e.surfEntries));
                        _containerSurfSpecialEntries.Add(new(e.surfSpecialEntries));
                        _containerFishEntries.Add(new(e.fishEntries));
                        _containerFishSpecialEntries.Add(new(e.fishSpecialEntries));
                    }
                    SelectedSubentryIndex = 0;
                }
            }
            catch (Exception ex) {
                MessageHandler.ErrorMessage("Invalid wild encounter container", "This file is invalid.");
            }
        }
        
        public override void OnAddNew() {
            
        }
        
        public override void OnRemoveSelected(int index) {
            
        }

        public override void OnSaveChanges() {
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < _numberOfEntries; ++i) {
                ms.Write(new WildEncounters {
                    grassEntries = new List<WildEncounterEntry>(_containerGrassEntries[i]),
                    grassDoubleEntries = new List<WildEncounterEntry>(_containerGrassDoubleEntries[i]),
                    grassSpecialEntries = new List<WildEncounterEntry>(_containerGrassSpecialEntries[i]),
                    surfEntries = new List<WildEncounterEntry>(_containerSurfEntries[i]),
                    surfSpecialEntries = new List<WildEncounterEntry>(_containerSurfSpecialEntries[i]),
                    fishEntries = new List<WildEncounterEntry>(_containerFishEntries[i]),
                    fishSpecialEntries = new List<WildEncounterEntry>(_containerFishSpecialEntries[i]),
                }.serialize());
            }
            UI.Patcher.saveToNarcFolder(UI.GameInfo.wildEncounters, SelectedIndex, x => File.WriteAllBytes(x, ms.ToArray()));
        }
    }
}