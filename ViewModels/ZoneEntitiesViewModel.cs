using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using BeaterLibrary.Formats.Furniture;
using BeaterLibrary.Formats.Scripts;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.Controls.ViewModels
{
    public class ZoneEntitiesViewModel : ViewModelBase
    {
        private Dictionary<int, ReactiveCommand<Unit, Unit>[]> FunctionMap;
        public ReactiveCommand<Unit, Unit>? AddNew { get; private set; }
        public ReactiveCommand<Unit, Unit> RemoveSelected { get; private set; }
        public ObservableCollection<Interactable> Interactables { get; }
        public ObservableCollection<NPC> NPCs { get; }
        public ObservableCollection<Warp> Warps { get; }
        public ObservableCollection<Trigger> Triggers { get; }
        public ObservableCollection<InitializationScript> InitializationScripts { get; }
        private int _SelectedTab;
        private int _SelectedRow;

        public int SelectedTab
        {
            get => _SelectedTab;
            set
            {
                AddNew = FunctionMap[value][0];
                this.RaisePropertyChanged("AddNew");
                RemoveSelected = FunctionMap[value][1];
                this.RaisePropertyChanged("RemoveSelected");
                this.RaiseAndSetIfChanged(ref _SelectedTab, value);
            }
        }

        public int SelectedRow
        {
            get => _SelectedRow;
            set => this.RaiseAndSetIfChanged(ref _SelectedRow, value);
        }

        public ZoneEntitiesViewModel()
        {
            Interactables = new ObservableCollection<Interactable>();
            NPCs = new ObservableCollection<NPC>();
            Warps = new ObservableCollection<Warp>();
            Triggers = new ObservableCollection<Trigger>();
            InitializationScripts = new ObservableCollection<InitializationScript>();

            FunctionMap = new Dictionary<int, ReactiveCommand<Unit, Unit>[]>
            {
                {
                    0,
                    new[]
                    {
                        ReactiveCommand.Create(() => Interactables.Add(new Interactable())),
                        ReactiveCommand.Create(() => Interactables.RemoveAt(SelectedRow))
                    }
                },
                {
                    1,
                    new[]
                    {
                        ReactiveCommand.Create(() => NPCs.Add(new NPC())),
                        ReactiveCommand.Create(() => NPCs.RemoveAt(SelectedRow))
                    }
                },
                {
                    2,
                    new[]
                    {
                        ReactiveCommand.Create(() => Warps.Add(new Warp())),
                        ReactiveCommand.Create(() => Warps.RemoveAt(SelectedRow))
                    }
                },
                {
                    3,
                    new[]
                    {
                        ReactiveCommand.Create(() => Triggers.Add(new Trigger())),
                        ReactiveCommand.Create(() => Triggers.RemoveAt(SelectedRow))
                    }
                },
                {
                    4,
                    new[]
                    {
                        ReactiveCommand.Create(() => InitializationScripts.Add(new InitializationScript())),
                        ReactiveCommand.Create(() => InitializationScripts.RemoveAt(SelectedRow))
                    }
                },
            };
            SelectedTab = 0;
        }

        public ZoneEntitiesViewModel(ZoneEntities Overworld) : this()
        {
            Interactables = new ObservableCollection<Interactable>(Overworld.Interactables);
            NPCs = new ObservableCollection<NPC>(Overworld.NPCs);
            Warps = new ObservableCollection<Warp>(Overworld.Warps);
            Triggers = new ObservableCollection<Trigger>(Overworld.Triggers);
            InitializationScripts = new ObservableCollection<InitializationScript>(Overworld.LevelScripts);
        }
    }
}