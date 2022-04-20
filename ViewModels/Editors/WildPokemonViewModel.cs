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

        private List<ObservableCollection<WildEncounterEntry>> containerGrassEntries, containerGrassDoubleEntries, containerGrassSpecialEntries;
        private List<ObservableCollection<WildEncounterEntry>> containerSurfEntries, containerSurfSpecialEntries;
        private List<ObservableCollection<WildEncounterEntry>> containerFishEntries, containerFishSpecialEntries;

        public ObservableCollection<WildEncounterEntry> grassSingleEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> grassDoubleEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> grassSpecialEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> surfEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> surfSpecialEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> fishEntries { get; set; }
        public ObservableCollection<WildEncounterEntry> fishSpecialEntries { get; set; }
        public ObservableCollection<string> speciesNames { get; set; }
        public ReactiveCommand<Unit, Unit> loadContainer { get; }
        public ReactiveCommand<Unit, Unit> loadSubentry { get; }

        public WildPokemonViewModel() {
            speciesNames = new ObservableCollection<string>(
                new TextContainer(UI.patcher.fetchFileFromNarc(UI.gameInfo.systemsText,
                    UI.gameInfo.ImportantSystemText["PokémonNames"])).fetchTextAsStringArray());
            this.RaisePropertyChanged(nameof(speciesNames));
            loadContainer = ReactiveCommand.Create(() => changeWildContainer(selectedIndex));
            loadSubentry = ReactiveCommand.Create(() => onSelectedSubentryChange(selectedSubentryIndex));
            containerGrassEntries = new List<ObservableCollection<WildEncounterEntry>>();
            containerGrassDoubleEntries = new List<ObservableCollection<WildEncounterEntry>>();
            containerGrassSpecialEntries = new List<ObservableCollection<WildEncounterEntry>>();
            containerFishEntries = new List<ObservableCollection<WildEncounterEntry>>();
            containerFishSpecialEntries = new List<ObservableCollection<WildEncounterEntry>>();
            containerSurfEntries = new List<ObservableCollection<WildEncounterEntry>>();
            containerSurfSpecialEntries = new List<ObservableCollection<WildEncounterEntry>>();
        }
        
        public override int selectedIndex {
            get => _selectedIndex;
            set => onIndexChange(value);
        }
        
        public int selectedSubentryIndex {
            get => _selectedSubEntry;
            set => onSelectedSubentryChange(value);
        }

        public override void onIndexChange(int newValue) {
            if (newValue >= 0 && newValue < UI.patcher.getNarcEntryCount(UI.gameInfo.wildEncounters)) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
            }
        }

        private void onSelectedSubentryChange(int newValue) {
            if (newValue >= 0 && newValue < _numberOfEntries) {
                this.RaiseAndSetIfChanged(ref _selectedSubEntry, newValue);
            }
            grassSingleEntries = containerGrassEntries[selectedSubentryIndex];
            grassDoubleEntries = containerGrassDoubleEntries[selectedSubentryIndex];
            grassSpecialEntries = containerGrassSpecialEntries[selectedSubentryIndex];
            surfEntries = containerSurfEntries[selectedSubentryIndex];
            surfSpecialEntries = containerSurfSpecialEntries[selectedSubentryIndex];
            fishEntries = containerFishEntries[selectedSubentryIndex];
            fishSpecialEntries = containerFishSpecialEntries[selectedSubentryIndex];
            
            this.RaisePropertyChanged(nameof(grassSingleEntries));
            this.RaisePropertyChanged(nameof(grassDoubleEntries));
            this.RaisePropertyChanged(nameof(grassSpecialEntries));
            this.RaisePropertyChanged(nameof(surfEntries));
            this.RaisePropertyChanged(nameof(surfSpecialEntries));
            this.RaisePropertyChanged(nameof(fishEntries));
            this.RaisePropertyChanged(nameof(fishSpecialEntries));
        }

        private void resetContainerEntries() {
            containerGrassEntries.Clear();
            containerGrassDoubleEntries.Clear();
            containerGrassSpecialEntries.Clear();
            containerFishEntries.Clear();
            containerFishSpecialEntries.Clear();
            containerSurfEntries.Clear();
            containerSurfSpecialEntries.Clear();
        }

        private void changeWildContainer(int index) {
            resetContainerEntries();
            try {
                byte[] buffer = UI.patcher.fetchFileFromNarc(UI.gameInfo.wildEncounters, index);
                if (buffer.Length > 0 && buffer.Length % 0xE8 == 0) {
                    _numberOfEntries = buffer.Length / 0xE8;
                    for (int Index = 0; Index < _numberOfEntries; ++Index) {
                        WildEncounters e = new(buffer.Skip(0xE8 * Index).Take(0xE8).ToArray());
                        containerGrassEntries.Add(new(e.grassEntries));
                        containerGrassDoubleEntries.Add(new(e.grassDoubleEntries));
                        containerGrassSpecialEntries.Add(new(e.grassSpecialEntries));
                        containerSurfEntries.Add(new(e.surfEntries));
                        containerSurfSpecialEntries.Add(new(e.surfSpecialEntries));
                        containerFishEntries.Add(new(e.fishEntries));
                        containerFishSpecialEntries.Add(new(e.fishSpecialEntries));
                    }
                    selectedSubentryIndex = 0;
                }
            }
            catch (Exception ex) {
                MessageHandler.errorMessage("Invalid wild encounter container", "This file is invalid.");
            }
        }
        
        public override void onAddNew() {
            
        }
        
        public override void onRemoveSelected(int index) {
            
        }

        public override void onSaveChanges() {
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < _numberOfEntries; ++i) {
                ms.Write(new WildEncounters {
                    grassEntries = new List<WildEncounterEntry>(containerGrassEntries[i]),
                    grassDoubleEntries = new List<WildEncounterEntry>(containerGrassDoubleEntries[i]),
                    grassSpecialEntries = new List<WildEncounterEntry>(containerGrassSpecialEntries[i]),
                    surfEntries = new List<WildEncounterEntry>(containerSurfEntries[i]),
                    surfSpecialEntries = new List<WildEncounterEntry>(containerSurfSpecialEntries[i]),
                    fishEntries = new List<WildEncounterEntry>(containerFishEntries[i]),
                    fishSpecialEntries = new List<WildEncounterEntry>(containerFishSpecialEntries[i]),
                }.serialize());
            }
            UI.patcher.saveToNarcFolder(UI.gameInfo.wildEncounters, selectedIndex, x => File.WriteAllBytes(x, ms.ToArray()));
        }
    }
}