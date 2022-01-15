using BeaterLibrary.Formats.Pokémon;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.Models {
    public class EncounterEntry : ReactiveObject {
        private ushort _speciesIndex, _form, _minLevel, _maxLevel;
        public ushort speciesIndex {
            get => _speciesIndex;
            set => this.RaiseAndSetIfChanged(ref _speciesIndex, value);
        }       
        public ushort form {
            get => _form;
            set => this.RaiseAndSetIfChanged(ref _form, value);
        }
        
        public ushort minLevel {
            get => _minLevel;
            set => this.RaiseAndSetIfChanged(ref _minLevel, value);
        }        
        public ushort maxLevel {
            get => _maxLevel;
            set => this.RaiseAndSetIfChanged(ref _maxLevel, value);
        }

        public EncounterEntry(WildEncounterEntry e) {
            speciesIndex = e.nationalDexNumber;
            form = e.formNumber;
            minLevel = e.minimumLevel;
            maxLevel = e.maximumLevel;
        }
    }
}