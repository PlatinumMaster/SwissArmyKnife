using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using BeaterLibrary.Formats.Overworld;
using BeaterLibrary.Formats.Scripts;
using DynamicData.Kernel;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.ViewModels
{
    public class ZoneEntitiesViewModel : ViewModelBase
    {
        private readonly Dictionary<int, ReactiveCommand<Unit, Unit>[]> FunctionMap;
        private int _SelectedContainer;
        private int _SelectedRow;
        private int _SelectedTab;

        public ZoneEntitiesViewModel()
        {
            Interactables = new ObservableCollection<Interactable>();
            NPCs = new ObservableCollection<NPC>();
            Warps = new ObservableCollection<Warp>();
            Triggers = new ObservableCollection<Trigger>();
            InitializationScripts = new ObservableCollection<InitializationScript>();
            TriggerRelatedData = new ObservableCollection<TriggerRelated>();
            SaveFile = ReactiveCommand.Create(() =>
            {
                UIUtil.BaseROMPatcher.SaveToNARCFolder(
                    UIUtil.CurrentGameInformation.ZoneEntities,
                    SelectedContainer,
                    x => ZoneEntities.Serialize(Interactables.AsList(), NPCs.AsList(),
                        Warps.AsList(), Triggers.AsList(), InitializationScripts.AsList(), TriggerRelatedData.AsList(),
                        x)
                );
            });
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
                {
                    5,
                    new[]
                    {
                        ReactiveCommand.Create(() => TriggerRelatedData.Add(new TriggerRelated())),
                        ReactiveCommand.Create(() => TriggerRelatedData.RemoveAt(SelectedRow))
                    }
                }
            };
            SelectedTab = 0;
        }

        public ZoneEntitiesViewModel(int Count) : this()
        {
            ZoneEntityNames = Enumerable.Range(0, Count - 1).Select(x => $"Container {x}").ToList();
        }

        public ReactiveCommand<Unit, Unit>? AddNew { get; private set; }
        public ReactiveCommand<Unit, Unit> RemoveSelected { get; private set; }
        public ObservableCollection<Interactable> Interactables { get; set; }
        public ObservableCollection<NPC> NPCs { get; set; }
        public ObservableCollection<Warp> Warps { get; set; }
        public ObservableCollection<Trigger> Triggers { get; set; }
        public ObservableCollection<InitializationScript> InitializationScripts { get; set; }
        public ObservableCollection<TriggerRelated> TriggerRelatedData { get; set; }
        public List<string> ZoneEntityNames { get; }
        public ReactiveCommand<Unit, Unit> SaveFile { get; set; }


        public int SelectedContainer
        {
            get => _SelectedContainer;
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedContainer, value);
                ZoneEntities Curr =
                    new(UIUtil.BaseROMPatcher.FetchFileFromNARC(UIUtil.CurrentGameInformation.ZoneEntities, value));
                Interactables = new ObservableCollection<Interactable>(Curr.Interactables);
                NPCs = new ObservableCollection<NPC>(Curr.NPCs);
                Warps = new ObservableCollection<Warp>(Curr.Warps);
                Triggers = new ObservableCollection<Trigger>(Curr.Triggers);
                InitializationScripts = new ObservableCollection<InitializationScript>(Curr.InitializationScripts);
                TriggerRelatedData = new ObservableCollection<TriggerRelated>(Curr.TriggerRelatedEntries);
                this.RaisePropertyChanged("Curr");
                this.RaisePropertyChanged("Interactables");
                this.RaisePropertyChanged("NPCs");
                this.RaisePropertyChanged("Warps");
                this.RaisePropertyChanged("Triggers");
                this.RaisePropertyChanged("InitializationScripts");
                this.RaisePropertyChanged("TriggerRelatedData");
            }
        }

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
    }
}