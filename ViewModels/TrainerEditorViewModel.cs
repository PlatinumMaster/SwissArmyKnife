using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using BeaterLibrary.Formats.Trainer;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.Controls.ViewModels
{
    public class TrainerEditorViewModel : ViewModelBase
    {
        private bool _EnableTrPokeButton;
        private int _SelectedEntry;
        private TrainerData _TrainerData;
        private TrainerPokémonEntry _CurrentEntry;
        public ObservableCollection<TrainerPokémonEntry> TrainerPokemon { get; set; }
        public ReactiveCommand<Unit, Unit>? LoadTrainerData { get; }
        public ReactiveCommand<Unit, Unit>? LoadTrainerPokemon { get; }
        public bool EnableTrPokeButton { get => _EnableTrPokeButton; private set => this.RaiseAndSetIfChanged(ref _EnableTrPokeButton, value); }
        public TrainerPokémonEntry CurrentEntry { get => _CurrentEntry; private set => this.RaiseAndSetIfChanged(ref _CurrentEntry, value); }
        public TrainerData TrainerData { get => _TrainerData; private set => this.RaiseAndSetIfChanged(ref _TrainerData, value); }
        
        public int SelectedEntry
        {
            get => _SelectedEntry;
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedEntry, value);
                CurrentEntry = TrainerPokemon[value];
            }
        }

        public TrainerEditorViewModel()
        {
            LoadTrainerData = ReactiveCommand.Create(() =>
            {
                UIUtil.HandleFile(false,
                    path =>
                    {
                        TrainerData = new TrainerData(new BinaryReader(File.OpenRead(path)));
                        EnableTrPokeButton = true;
                    }, null, null);
            });

            LoadTrainerPokemon = ReactiveCommand.Create(() =>
            {
                UIUtil.HandleFile(false,
                    path =>
                    {
                        TrainerPokemon = new ObservableCollection<TrainerPokémonEntry>(new TrainerPokémonEntries(
                            new BinaryReader(File.OpenRead(path)), TrainerData.HasItem, TrainerData.HasMoves,
                            TrainerData.NumberOfPokemon).PokémonEntries);
                        SelectedEntry = 0;
                    }, null, null);
            });
            TrainerData = new TrainerData();
        }
    }
}