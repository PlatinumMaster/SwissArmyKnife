using System;
using System.Collections.ObjectModel;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.Formats.Trainer;
using BeaterLibrary.GameInfo;
using ReactiveUI;
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
        public ObservableCollection<string> AbilityChoices { get; private set; }
        public ObservableCollection<string> Genders { get; private set; }

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
                this.RaisePropertyChanged("aiFlags");
            }
        }

        public TrainerData currentTrainer {
            get => _currentTrainer;
            private set => this.RaiseAndSetIfChanged(ref _currentTrainer, value);
        }

        public TrainerPokémonEntries currentPkmnEntries {
            get => _currentPkmnEntries;
            private set {
                this.RaiseAndSetIfChanged(ref _currentPkmnEntries, value);
                this.RaisePropertyChanged(nameof(currentPkmnEntries));
            }
        }

        public TrainerPokémonEntry currentPkmnEntry { get; private set; }
        
        public TrainerEditorViewModel() {
            fetchAllTextArchives();
            selectedIndex = 0;
        }

        public override void onAddNew() {
            
        }

        public override void onIndexChange(int newValue) {
            if (newValue < TrainerNames.Count && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
                fetchTrainerData(selectedIndex);
                selectedPkmnEntryIndex = 0;
            }
        }

        public override void onRemoveSelected(int index) {
            
        }

        public override void onSaveChanges() {
            UI.patcher.saveToNarcFolder(UI.gameInfo.trainerPokemon, selectedIndex, x => currentPkmnEntries.serialize(setPkmnMoves, setPkmnMoves, x));
            UI.patcher.saveToNarcFolder(UI.gameInfo.trainerData, selectedIndex, x => currentTrainer.serialize(currentPkmnEntries.pokémonEntries, x));
        }

        public void onPkmnIndexChange(int newValue) {
            if (newValue < currentTrainer.numberOfPokemon && newValue >= 0) {
                this.RaiseAndSetIfChanged(ref _selectedPkmnEntryIndex, newValue);
                currentPkmnEntry = currentPkmnEntries.pokémonEntries[newValue];
                this.RaisePropertyChanged("selectedPkmnEntryIndex");
                this.RaisePropertyChanged("currentPkmnEntry");
            }
        }

        private ObservableCollection<string> fetchTextArchive(B2W2.ImportantSystemText bank) {
            return new ObservableCollection<string>(new TextContainer(UI.patcher.fetchFileFromNarc(UI.gameInfo.systemsText, (int) bank)).fetchTextAsStringArray());
        }

        private void fetchTrainerData(int index) {
            currentTrainer = new TrainerData(UI.patcher.fetchFileFromNarc(UI.gameInfo.trainerData, index));
            currentPkmnEntries = new TrainerPokémonEntries(
                UI.patcher.fetchFileFromNarc(UI.gameInfo.trainerPokemon, index),
                currentTrainer.setPkmnMoves, currentTrainer.numberOfPokemon, currentTrainer.setPkmnHeldItem
            );
            this.RaisePropertyChanged(nameof(setPkmnMoves));
            this.RaisePropertyChanged(nameof(setPkmnHeldItem));
            bool[] __aiFlags = new bool[8];
            for (int Index = 0; Index < 8; ++Index) {
                __aiFlags[Index] = (currentTrainer.ai & (1 << Index)) == 1;
            }
            aiFlags = __aiFlags;
        }

        private void fetchAllTextArchives() {
            TrainerNames = fetchTextArchive(B2W2.ImportantSystemText.TrainerNames);
            TrainerClasses = fetchTextArchive(B2W2.ImportantSystemText.TrainerClasses);
            BattleTypes = fetchTextArchive(B2W2.ImportantSystemText.BattleTypes);
            ItemNames = fetchTextArchive(B2W2.ImportantSystemText.ItemNames);
            MoveNames = fetchTextArchive(B2W2.ImportantSystemText.MoveNames);
            PokémonNames = fetchTextArchive(B2W2.ImportantSystemText.PokémonNames);
            AbilityChoices = new ObservableCollection<string> {
                "Random",
                "Primary",
                "Secondary",
                "Hidden"
            };
            Genders = new ObservableCollection<string> {
                "Random",
                "Male",
                "Female"
            };
        }
    }
}