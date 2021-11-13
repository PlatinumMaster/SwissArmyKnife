using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using BeaterLibrary.Formats.Trainer;
using DynamicData.Kernel;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.ViewModels
{
    public class TrainerEditorViewModel : ViewModelBase
    {
        private BitArray _AIBuffer = new(8);
        private TrainerPokémonEntry _CurrentEntry;
        private TrainerData _CurrentTrainerData;
        private bool _EnableItems;
        private bool _EnableMoves;
        private int _SelectedPokémonEntry;
        private int _SelectedTrainer;

        public TrainerEditorViewModel(List<string> TrainerNames,
            List<string> TrainerClassNames, List<string> BattleTypes, List<string> ItemNames, List<string> MoveNames,
            List<string> SpeciesNames, List<string> AbilityNames, List<string> GenderNames)
        {
            this.TrainerNames = TrainerNames;
            this.TrainerClassNames = TrainerClassNames;
            this.BattleTypes = BattleTypes;
            this.ItemNames = ItemNames;
            this.MoveNames = MoveNames;
            this.SpeciesNames = SpeciesNames;
            this.AbilityNames = AbilityNames;
            this.GenderNames = GenderNames;
            CurrentTrainerPokemonEntries = new ObservableCollection<TrainerPokémonEntry>();
            AddNew = ReactiveCommand.Create(() =>
            {
                if (CurrentTrainerPokemonEntries.Count < 6)
                {
                    CurrentTrainerPokemonEntries.Add(
                        new TrainerPokémonEntry(CurrentTrainerData.OverrideMoves, CurrentTrainerData.HasItems));
                    if (CurrentTrainerPokemonEntries.Count == 1)
                        SelectedPokémonEntry = 0;
                }
                else
                {
                    MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Looks like we found Ghetsis...",
                            ContentMessage = "Fuck outta here Ghetsis",
                            Icon = Icon.Error,
                            Style = Style.None
                        }).Show();
                }
            });
            RemoveSelected = ReactiveCommand.Create(() =>
            {
                if (SelectedPokémonEntry != -1)
                    CurrentTrainerPokemonEntries.RemoveAt(SelectedPokémonEntry);
            });
            SaveFile = ReactiveCommand.Create(() =>
            {
                UIUtil.BaseROMPatcher.SaveToNARCFolder(UIUtil.CurrentGameInformation.TrainerPokemon, SelectedTrainer,
                    x => TrainerPokémonEntries.Serialize(CurrentTrainerPokemonEntries.AsList(), MovesEnabled,
                        ItemsEnabled,
                        x));
                CurrentTrainerData.AI = BitArrToByte(_AIBuffer);
                UIUtil.BaseROMPatcher.SaveToNARCFolder(UIUtil.CurrentGameInformation.TrainerData, SelectedTrainer,
                    x => CurrentTrainerData.Serialize(CurrentTrainerPokemonEntries.AsList(),
                        x));
            });
            SelectedTrainer = 0;
        }

        public TrainerEditorViewModel()
        {
        }

        public ObservableCollection<TrainerPokémonEntry> CurrentTrainerPokemonEntries { get; set; }
        public List<string> TrainerClassNames { get; }
        public List<string> TrainerNames { get; }
        public List<string> BattleTypes { get; }
        public List<string> ItemNames { get; }
        public List<string> MoveNames { get; }
        public List<string> SpeciesNames { get; }
        public List<string> AbilityNames { get; set; }
        public List<string> GenderNames { get; }
        public ReactiveCommand<Unit, Unit> RemoveSelected { get; set; }
        public ReactiveCommand<Unit, Unit> AddNew { get; set; }
        public ReactiveCommand<Unit, Unit> SaveFile { get; set; }

        public TrainerData CurrentTrainerData
        {
            get => _CurrentTrainerData;
            set => this.RaiseAndSetIfChanged(ref _CurrentTrainerData, value);
        }

        public bool ItemsEnabled
        {
            get => _EnableItems;
            set => this.RaiseAndSetIfChanged(ref _EnableItems, value);
        }

        public bool MovesEnabled
        {
            get => _EnableMoves;
            set => this.RaiseAndSetIfChanged(ref _EnableMoves, value);
        }

        public TrainerPokémonEntry CurrentEntry
        {
            get => _CurrentEntry;
            set => this.RaiseAndSetIfChanged(ref _CurrentEntry, value);
        }

        public int SelectedTrainer
        {
            get => _SelectedTrainer;
            set
            {
                CurrentTrainerData = new TrainerData(new BinaryReader(new MemoryStream(
                    UIUtil.BaseROMPatcher.FetchFileFromNARC(UIUtil.CurrentGameInformation.TrainerData, value))));
                CurrentTrainerPokemonEntries =
                    new ObservableCollection<TrainerPokémonEntry>(new TrainerPokémonEntries(new BinaryReader(
                            new MemoryStream(
                                UIUtil.BaseROMPatcher.FetchFileFromNARC(UIUtil.CurrentGameInformation.TrainerPokemon,
                                    value))),
                        CurrentTrainerData.OverrideMoves, CurrentTrainerData.NumberOfPokemon,
                        CurrentTrainerData.HasItems).PokémonEntries);
                this.RaiseAndSetIfChanged(ref _SelectedTrainer, value);
                this.RaisePropertyChanged("CurrentTrainerPokemonEntries");
                MovesEnabled = CurrentTrainerData.OverrideMoves;
                ItemsEnabled = CurrentTrainerData.HasItems;
                _AIBuffer = ByteToBitArr((byte) CurrentTrainerData.AI);
                RaiseAllBitsChanged();
                SelectedPokémonEntry = 0;
            }
        }

        public int SelectedPokémonEntry
        {
            get => _SelectedPokémonEntry;
            set
            {
                if (CurrentTrainerPokemonEntries.Count > 0)
                {
                    this.RaiseAndSetIfChanged(ref _SelectedPokémonEntry, value);
                    this.RaiseAndSetIfChanged(ref _CurrentEntry, CurrentTrainerPokemonEntries[value]);
                    this.RaisePropertyChanged("CurrentEntry");
                }
            }
        }

        public bool Bit0Checked
        {
            get => _AIBuffer[0];
            set => SetBit(0, value);
        }

        public bool Bit1Checked
        {
            get => _AIBuffer[1];
            set => SetBit(1, value);
        }

        public bool Bit2Checked
        {
            get => _AIBuffer[2];
            set => SetBit(2, value);
        }

        public bool Bit3Checked
        {
            get => _AIBuffer[3];
            set => SetBit(3, value);
        }

        public bool Bit4Checked
        {
            get => _AIBuffer[4];
            set => SetBit(4, value);
        }

        public bool Bit5Checked
        {
            get => _AIBuffer[5];
            set => SetBit(5, value);
        }

        public bool Bit6Checked
        {
            get => _AIBuffer[6];
            set => SetBit(6, value);
        }

        public bool Bit7Checked
        {
            get => _AIBuffer[7];
            set => SetBit(7, value);
        }

        private BitArray ByteToBitArr(byte Val)
        {
            return new(BitConverter.GetBytes(Val));
        }

        private byte BitArrToByte(BitArray B)
        {
            byte Val = 0;
            for (var i = 0; i < B.Count; ++i)
                if (B[i])
                    Val |= (byte) (1 << i);
            return Val;
        }

        private void SetBit(int Index, bool NewValue)
        {
            _AIBuffer[Index] = NewValue;
            this.RaisePropertyChanged($"Bit{Index}Checked");
        }

        private void RaiseAllBitsChanged()
        {
            for (var i = 0; i < _AIBuffer.Count; ++i)
                this.RaisePropertyChanged($"Bit{i}Checked");
        }
    }
}