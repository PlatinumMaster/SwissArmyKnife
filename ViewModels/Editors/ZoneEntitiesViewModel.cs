using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using BeaterLibrary.Formats.Overworld;
using BeaterLibrary.Formats.Scripts;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public class ZoneEntitiesViewModel : ViewModelTemplate {
        private const int INTERACTABLES = 0,
            NPC = 1,
            WARPS = 2,
            TRIGGERS = 3,
            STATIC_INIT_SCRIPTS = 4,
            DYNAMIC_INIT_SCRIPTS = 5;
        
        private int _selectedIndex;
        private ZoneEntities _currentZoneEntities;
        private int[] _selectedSubIndices;
        public ObservableCollection<Interactable> Interactables { get; set; }
        public ObservableCollection<NPC> NPCs { get; set; }
        public ObservableCollection<Warp> Warps { get; set; }
        public ObservableCollection<Trigger> Triggers { get; set; }
        public ObservableCollection<InitializationScript> InitScripts { get; set; }
        public ObservableCollection<TriggerRelated> TriggerRelated { get; set; }

        public ReactiveCommand<Unit, Unit> OnAddNewListEntry { get; }
        public ReactiveCommand<Unit, Unit> OnRemoveSelectedListEntry { get; }

        public ZoneEntities CurrentZoneEntities {
            get => _currentZoneEntities;
            set {
                this.RaiseAndSetIfChanged(ref _currentZoneEntities, value);
                this.RaisePropertyChanged();
            }
        }
        public int SelectedTab { get; set; }
        public override int SelectedIndex {
            get => _selectedIndex;
            set => OnIndexChange(value);
        }

        public int[] SelectedSubIndices {
            get => _selectedSubIndices;
            set {
                this.RaiseAndSetIfChanged(ref _selectedSubIndices, value);
                this.RaisePropertyChanged();
            }
        }

        public ZoneEntitiesViewModel() {
            OnAddNewListEntry = ReactiveCommand.Create(OnAddNewEntry);
            OnRemoveSelectedListEntry = ReactiveCommand.Create(OnRemoveSelectedEntry);
            SelectedSubIndices = new int[6];
            SelectedTab = 0;
            SelectedIndex = 0;
        }

        public override void OnAddNew() {
            
        }

        public override void OnRemoveSelected(int index) {
            
        }

        public void OnAddNewEntry() {
            switch (SelectedTab) {
                case INTERACTABLES:
                    Interactables.Add(new Interactable());
                    this.RaisePropertyChanged(nameof(Interactables));
                    break;
                case NPC:
                    NPCs.Add(new NPC());
                    this.RaisePropertyChanged(nameof(NPCs));
                    break;
                case WARPS:
                    Warps.Add(new Warp());
                    this.RaisePropertyChanged(nameof(Warps));
                    break;
                case TRIGGERS:
                    Triggers.Add(new Trigger());
                    this.RaisePropertyChanged(nameof(Triggers));
                    break;
                case STATIC_INIT_SCRIPTS:
                    InitScripts.Add(new InitializationScript());
                    this.RaisePropertyChanged(nameof(InitScripts));
                    break;
                case DYNAMIC_INIT_SCRIPTS:
                    TriggerRelated.Add(new TriggerRelated());
                    this.RaisePropertyChanged(nameof(TriggerRelated));
                    break;
            }
        }
        
        public override void OnIndexChange(int newValue) {
            if (newValue >= 0 && newValue < UI.Patcher.getNarcEntryCount(UI.GameInfo.zoneEntities)) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
                this.RaisePropertyChanged(nameof(SelectedIndex));

                CurrentZoneEntities = new ZoneEntities(UI.Patcher.fetchFileFromNarc(UI.GameInfo.zoneEntities, newValue));
                Interactables = new ObservableCollection<Interactable>(CurrentZoneEntities.interactables);
                NPCs = new ObservableCollection<NPC>(CurrentZoneEntities.npcs);
                Warps = new ObservableCollection<Warp>(CurrentZoneEntities.warps);
                Triggers = new ObservableCollection<Trigger>(CurrentZoneEntities.triggers);
                InitScripts = new ObservableCollection<InitializationScript>(CurrentZoneEntities.initializationScripts);
                TriggerRelated = new ObservableCollection<TriggerRelated>(CurrentZoneEntities.triggerRelatedEntries);
                
                this.RaisePropertyChanged(nameof(Interactables));
                this.RaisePropertyChanged(nameof(NPCs));
                this.RaisePropertyChanged(nameof(Warps));
                this.RaisePropertyChanged(nameof(Triggers));
                this.RaisePropertyChanged(nameof(InitScripts));
                this.RaisePropertyChanged(nameof(TriggerRelated));
            }
        }

        public void OnRemoveSelectedEntry() {
            if (SelectedSubIndices[SelectedTab] > -1) {
                switch (SelectedTab) {
                    case INTERACTABLES:
                        Interactables.RemoveAt(SelectedSubIndices[SelectedTab]);
                        this.RaisePropertyChanged(nameof(Interactables));
                        break;
                    case NPC:
                        NPCs.RemoveAt(SelectedSubIndices[SelectedTab]);
                        this.RaisePropertyChanged(nameof(NPCs));
                        break;
                    case WARPS:
                        Warps.RemoveAt(SelectedSubIndices[SelectedTab]);
                        this.RaisePropertyChanged(nameof(Warps));
                        break;
                    case TRIGGERS:
                        Triggers.RemoveAt(SelectedSubIndices[SelectedTab]);
                        this.RaisePropertyChanged(nameof(Triggers));
                        break;
                    case STATIC_INIT_SCRIPTS:
                        InitScripts.RemoveAt(SelectedSubIndices[SelectedTab]);
                        this.RaisePropertyChanged(nameof(InitScripts));
                        break;
                    case DYNAMIC_INIT_SCRIPTS:
                        TriggerRelated.RemoveAt(SelectedSubIndices[SelectedTab]);
                        this.RaisePropertyChanged(nameof(TriggerRelated));
                        break;
                }
            }
        }

        public override void OnSaveChanges() {
            CurrentZoneEntities.interactables = new List<Interactable>(Interactables);
            CurrentZoneEntities.npcs = new List<NPC>(NPCs);
            CurrentZoneEntities.warps = new List<Warp>(Warps);
            CurrentZoneEntities.triggers = new List<Trigger>(Triggers);
            CurrentZoneEntities.initializationScripts = new List<InitializationScript>(InitScripts);
            CurrentZoneEntities.triggerRelatedEntries = new List<TriggerRelated>(TriggerRelated);
        }
    }
}