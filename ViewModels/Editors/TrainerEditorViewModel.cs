using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Security.Cryptography;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.Formats.Trainer;
using BeaterLibrary.GameInfo;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public class TrainerEditorViewModel : ViewModelTemplate {
        private TrainerPokémonEntries _currentPkmnEntries;
        private TrainerData _currentTrainer;
        private int _selectedIndex;
        private int _selectedPkmnEntryIndex;
        private bool[] _aiFlags;
        public ObservableCollection<string> TrainerClasses { get; private set; }
        public ObservableCollection<string> TrainerNames { get; private set; }
        public ObservableCollection<string> BattleTypes { get; private set; }
        public ObservableCollection<string> ItemNames { get; private set; }
        public ObservableCollection<string> MoveNames { get; private set; }
        public ObservableCollection<string> PokémonNames { get; private set; }

        public override int selectedIndex {
            get => _selectedIndex;
            set => onIndexChange(value);
        }

        public int selectedPkmnEntryIndex {
            get => _selectedPkmnEntryIndex;
            set => onPkmnIndexChange(value);
        }

        public bool setPkmnMoves {
            get => currentTrainer.setPkmnMoves;
            set {
                currentTrainer.setPkmnMoves = value;
                this.RaisePropertyChanged();
            }
        }

        public bool setPkmnHeldItem {
            get => currentTrainer.setPkmnHeldItem;
            set {
                currentTrainer.setPkmnHeldItem = value;
                this.RaisePropertyChanged();
            }
        }

        public bool[] aiFlags {
            get => _aiFlags;
            set {
                this.RaiseAndSetIfChanged(ref _aiFlags, value);
                this.RaisePropertyChanged();
            }
        }

        public TrainerData currentTrainer {
            get => _currentTrainer;
            private set => this.RaiseAndSetIfChanged(ref _currentTrainer, value);
        }

        public ObservableCollection<TrainerPokémonEntry> currentPkmnEntries { get; set; }
        public ReactiveCommand<Unit, Unit> addNewTrainerPoke { get; }
        public ReactiveCommand<Unit, Unit> removeSelectedTrainerPoke { get; }
        public ReactiveCommand<Unit, Unit> loadTrainer { get; }
        public ReactiveCommand<Unit, Unit> reloadTextBanks { get; }
        public TrainerEditorViewModel() {
            fetchAllTextArchives();
            addNewTrainerPoke = ReactiveCommand.Create(onAddNewTrainerPoke);
            removeSelectedTrainerPoke = ReactiveCommand.Create(onRemoveTrainerPoke);
            loadTrainer = ReactiveCommand.Create(onLoadTrainer);
            reloadTextBanks = ReactiveCommand.Create(fetchAllTextArchives);
            selectedIndex = 0;
            onLoadTrainer();
        }

        private void onLoadTrainer() {
            fetchTrainerData(selectedIndex);
            selectedPkmnEntryIndex = 0;
        }
        public void onAddNewTrainerPoke() {
            if (currentPkmnEntries.Count == 6) {
                MessageHandler.errorMessage("Ghetsis detected", "You can only have 6 Pokémon.");
                return;
            }
            
            if (selectedPkmnEntryIndex > 0) {
                currentPkmnEntries.Insert(selectedPkmnEntryIndex + 1, new TrainerPokémonEntry());
            } else {
              currentPkmnEntries.Add(new TrainerPokémonEntry());
            }
            this.RaisePropertyChanged(nameof(currentPkmnEntries));
        }
        
        public void onRemoveTrainerPoke() {
            if (currentPkmnEntries.Count == 0) {
                MessageHandler.errorMessage("No Pokémon", "You need to have a Pokémon in order to remove it.");
                return;
            }

            if (selectedPkmnEntryIndex == -1) {
                MessageHandler.errorMessage("No Pokémon selected", "You need to select a Pokémon entry to remove.");
                return;
            }
            currentPkmnEntries.RemoveAt(selectedPkmnEntryIndex--);
            this.RaisePropertyChanged(nameof(currentPkmnEntries));
        }

        public override void onAddNew() {}

        public override void onIndexChange(int newValue) {
            if (newValue < TrainerNames.Count && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
                this.RaisePropertyChanged(nameof(selectedIndex));
            }
        }

        public override void onRemoveSelected(int index) {}

        public override void onSaveChanges() {
            currentTrainer.ai = 0;
            for (int Index = 0; Index < 8; ++Index) {
                currentTrainer.ai |= (uint)(aiFlags[Index] ? 1 : 0) << Index;
            }
            UI.patcher.saveToNarcFolder(UI.gameInfo.trainerPokemon, selectedIndex, x => TrainerPokémonEntries.serialize(new List<TrainerPokémonEntry>(currentPkmnEntries), setPkmnMoves, setPkmnHeldItem, x));
            UI.patcher.saveToNarcFolder(UI.gameInfo.trainerData, selectedIndex, x => currentTrainer.serialize(new List<TrainerPokémonEntry>(currentPkmnEntries), x));
        }

        public void onPkmnIndexChange(int newValue) {
            if (newValue < currentTrainer.numberOfPokemon && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedPkmnEntryIndex, newValue);
            }
        }

        private ObservableCollection<string> fetchTextArchive(int bank) {
            return new ObservableCollection<string>(new TextContainer(UI.patcher.fetchFileFromNarc(UI.gameInfo.systemsText, bank)).fetchTextAsStringArray());
        }

        private void fetchTrainerData(int index) {
            currentTrainer = new TrainerData(UI.patcher.fetchFileFromNarc(UI.gameInfo.trainerData, index));
            currentPkmnEntries = new ObservableCollection<TrainerPokémonEntry>(new TrainerPokémonEntries(
                UI.patcher.fetchFileFromNarc(UI.gameInfo.trainerPokemon, index),
                currentTrainer.setPkmnMoves, currentTrainer.numberOfPokemon, currentTrainer.setPkmnHeldItem
            ).pokémonEntries);
            this.RaisePropertyChanged(nameof(currentPkmnEntries));
            this.RaisePropertyChanged(nameof(setPkmnMoves));
            this.RaisePropertyChanged(nameof(setPkmnHeldItem));
            var _flags = new bool[8];
            for (int Index = 0; Index < 8; ++Index) {
                _flags[Index] = (currentTrainer.ai & (1 << Index)) == 1 << Index;
            }
            aiFlags = _flags;
        }

        private void fetchAllTextArchives() {
            TrainerNames = fetchTextArchive(UI.gameInfo.ImportantSystemText["TrainerNames"]);
            TrainerClasses = fetchTextArchive(UI.gameInfo.ImportantSystemText["TrainerClasses"]);
            BattleTypes = fetchTextArchive(UI.gameInfo.ImportantSystemText["BattleTypes"]);
            ItemNames = fetchTextArchive(UI.gameInfo.ImportantSystemText["ItemNames"]);
            MoveNames = fetchTextArchive(UI.gameInfo.ImportantSystemText["MoveNames"]);
            PokémonNames = fetchTextArchive(UI.gameInfo.ImportantSystemText["PokémonNames"]);
            this.RaisePropertyChanged(nameof(TrainerNames));
            this.RaisePropertyChanged(nameof(TrainerClasses));
            this.RaisePropertyChanged(nameof(BattleTypes));
            this.RaisePropertyChanged(nameof(ItemNames));
            this.RaisePropertyChanged(nameof(MoveNames));
            this.RaisePropertyChanged(nameof(PokémonNames));
        }
    }
}