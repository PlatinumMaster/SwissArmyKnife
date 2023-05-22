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

        public override int SelectedIndex {
            get => _selectedIndex;
            set => OnIndexChange(value);
        }

        public int SelectedPkmnEntryIndex {
            get => _selectedPkmnEntryIndex;
            set => OnPkmnIndexChange(value);
        }

        public bool SetPkmnMoves {
            get => CurrentTrainer.setPkmnMoves;
            set {
                CurrentTrainer.setPkmnMoves = value;
                this.RaisePropertyChanged();
            }
        }

        public bool SetPkmnHeldItem {
            get => CurrentTrainer.setPkmnHeldItem;
            set {
                CurrentTrainer.setPkmnHeldItem = value;
                this.RaisePropertyChanged();
            }
        }

        public bool[] AiFlags {
            get => _aiFlags;
            set {
                this.RaiseAndSetIfChanged(ref _aiFlags, value);
                this.RaisePropertyChanged();
            }
        }

        public TrainerData CurrentTrainer {
            get => _currentTrainer;
            private set => this.RaiseAndSetIfChanged(ref _currentTrainer, value);
        }

        public ObservableCollection<TrainerPokémonEntry> CurrentPkmnEntries { get; set; }
        public ReactiveCommand<Unit, Unit> AddNewTrainerPoke { get; }
        public ReactiveCommand<Unit, Unit> RemoveSelectedTrainerPoke { get; }
        public ReactiveCommand<Unit, Unit> LoadTrainer { get; }
        public ReactiveCommand<Unit, Unit> ReloadTextBanks { get; }
        public TrainerEditorViewModel() {
            FetchAllTextArchives();
            AddNewTrainerPoke = ReactiveCommand.Create(OnAddNewTrainerPoke);
            RemoveSelectedTrainerPoke = ReactiveCommand.Create(OnRemoveTrainerPoke);
            LoadTrainer = ReactiveCommand.Create(OnLoadTrainer);
            ReloadTextBanks = ReactiveCommand.Create(FetchAllTextArchives);
            SelectedIndex = 0;
            OnLoadTrainer();
        }

        private void OnLoadTrainer() {
            FetchTrainerData(SelectedIndex);
            SelectedPkmnEntryIndex = 0;
        }
        public void OnAddNewTrainerPoke() {
            if (CurrentPkmnEntries.Count == 6) {
                MessageHandler.ErrorMessage("Ghetsis detected", "You can only have 6 Pokémon.");
                return;
            }
            
            if (SelectedPkmnEntryIndex > 0) {
                CurrentPkmnEntries.Insert(SelectedPkmnEntryIndex + 1, new TrainerPokémonEntry());
            } else {
              CurrentPkmnEntries.Add(new TrainerPokémonEntry());
            }
            this.RaisePropertyChanged(nameof(CurrentPkmnEntries));
        }
        
        public void OnRemoveTrainerPoke() {
            if (CurrentPkmnEntries.Count == 0) {
                MessageHandler.ErrorMessage("No Pokémon", "You need to have a Pokémon in order to remove it.");
                return;
            }

            if (SelectedPkmnEntryIndex == -1) {
                MessageHandler.ErrorMessage("No Pokémon selected", "You need to select a Pokémon entry to remove.");
                return;
            }
            CurrentPkmnEntries.RemoveAt(SelectedPkmnEntryIndex--);
            this.RaisePropertyChanged(nameof(CurrentPkmnEntries));
        }

        public override void OnAddNew() {}

        public override void OnIndexChange(int newValue) {
            if (newValue < TrainerNames.Count && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
                this.RaisePropertyChanged(nameof(SelectedIndex));
            }
        }

        public override void OnRemoveSelected(int index) {}

        public override void OnSaveChanges() {
            CurrentTrainer.ai = 0;
            for (int index = 0; index < 8; ++index) {
                CurrentTrainer.ai |= (uint)(AiFlags[index] ? 1 : 0) << index;
            }
            UI.Patcher.saveToNarcFolder(UI.GameInfo.trainerPokemon, SelectedIndex, x => TrainerPokémonEntries.serialize(new List<TrainerPokémonEntry>(CurrentPkmnEntries), SetPkmnMoves, SetPkmnHeldItem, x));
            UI.Patcher.saveToNarcFolder(UI.GameInfo.trainerData, SelectedIndex, x => CurrentTrainer.serialize(new List<TrainerPokémonEntry>(CurrentPkmnEntries), x));
        }

        public void OnPkmnIndexChange(int newValue) {
            if (newValue < CurrentTrainer.numberOfPokemon && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedPkmnEntryIndex, newValue);
            }
        }

        private ObservableCollection<string> FetchTextArchive(int bank) {
            return new ObservableCollection<string>(new TextContainer(UI.Patcher.fetchFileFromNarc(UI.GameInfo.systemsText, bank)).fetchTextAsStringArray());
        }

        private void FetchTrainerData(int Index) {
            CurrentTrainer = new TrainerData(UI.Patcher.fetchFileFromNarc(UI.GameInfo.trainerData, Index));
            CurrentPkmnEntries = new ObservableCollection<TrainerPokémonEntry>(new TrainerPokémonEntries(
                UI.Patcher.fetchFileFromNarc(UI.GameInfo.trainerPokemon, Index),
                CurrentTrainer.setPkmnMoves, CurrentTrainer.numberOfPokemon, CurrentTrainer.setPkmnHeldItem
            ).pokémonEntries);
            this.RaisePropertyChanged(nameof(CurrentPkmnEntries));
            this.RaisePropertyChanged(nameof(SetPkmnMoves));
            this.RaisePropertyChanged(nameof(SetPkmnHeldItem));
            var flags = new bool[8];
            for (int index = 0; index < 8; ++index) {
                flags[index] = (CurrentTrainer.ai & (1 << index)) == 1 << index;
            }
            AiFlags = flags;
        }

        private void FetchAllTextArchives() {
            TrainerNames = FetchTextArchive(UI.GameInfo.ImportantSystemText["TrainerNames"]);
            TrainerClasses = FetchTextArchive(UI.GameInfo.ImportantSystemText["TrainerClasses"]);
            BattleTypes = FetchTextArchive(UI.GameInfo.ImportantSystemText["BattleTypes"]);
            ItemNames = FetchTextArchive(UI.GameInfo.ImportantSystemText["ItemNames"]);
            MoveNames = FetchTextArchive(UI.GameInfo.ImportantSystemText["MoveNames"]);
            PokémonNames = FetchTextArchive(UI.GameInfo.ImportantSystemText["PokémonNames"]);
            this.RaisePropertyChanged(nameof(TrainerNames));
            this.RaisePropertyChanged(nameof(TrainerClasses));
            this.RaisePropertyChanged(nameof(BattleTypes));
            this.RaisePropertyChanged(nameof(ItemNames));
            this.RaisePropertyChanged(nameof(MoveNames));
            this.RaisePropertyChanged(nameof(PokémonNames));
        }
    }
}