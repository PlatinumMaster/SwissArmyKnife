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
        private const int Interactables = 0,
            NPCs = 1,
            Warps = 2,
            Triggers = 3,
            InitializationScripts = 4,
            TriggerRelated = 5;
        
        private int _selectedIndex;
        private ZoneEntities _currentZoneEntities;
        private int[] _selectedSubIndices;
        public ObservableCollection<Interactable> interactables { get; set; }
        public ObservableCollection<NPC> npcs { get; set; }
        public ObservableCollection<Warp> warps { get; set; }
        public ObservableCollection<Trigger> triggers { get; set; }
        public ObservableCollection<InitializationScript> initScripts { get; set; }
        public ObservableCollection<TriggerRelated> triggerRelated { get; set; }

        public ReactiveCommand<Unit, Unit> onAddNewListEntry { get; }
        public ReactiveCommand<Unit, Unit> onRemoveSelectedListEntry { get; }

        public ZoneEntities currentZoneEntities {
            get => _currentZoneEntities;
            set {
                this.RaiseAndSetIfChanged(ref _currentZoneEntities, value);
                this.RaisePropertyChanged();
            }
        }
        public int selectedTab { get; set; }
        public override int selectedIndex {
            get => _selectedIndex;
            set => onIndexChange(value);
        }

        public int[] selectedSubIndices {
            get => _selectedSubIndices;
            set {
                this.RaiseAndSetIfChanged(ref _selectedSubIndices, value);
                this.RaisePropertyChanged();
            }
        }

        public ZoneEntitiesViewModel() {
            onAddNewListEntry = ReactiveCommand.Create(onAddNewEntry);
            onRemoveSelectedListEntry = ReactiveCommand.Create(onRemoveSelectedEntry);
            selectedSubIndices = new int[6];
            selectedTab = 0;
            selectedIndex = 0;
        }

        public override void onAddNew() {
            
        }

        public override void onRemoveSelected(int index) {
            
        }

        public void onAddNewEntry() {
            switch (selectedTab) {
                case Interactables:
                    interactables.Add(new Interactable());
                    this.RaisePropertyChanged(nameof(interactables));
                    break;
                case NPCs:
                    npcs.Add(new NPC());
                    this.RaisePropertyChanged(nameof(npcs));
                    break;
                case Warps:
                    warps.Add(new Warp());
                    this.RaisePropertyChanged(nameof(warps));
                    break;
                case Triggers:
                    triggers.Add(new Trigger());
                    this.RaisePropertyChanged(nameof(triggers));
                    break;
                case InitializationScripts:
                    initScripts.Add(new InitializationScript());
                    this.RaisePropertyChanged(nameof(initScripts));
                    break;
                case TriggerRelated:
                    triggerRelated.Add(new TriggerRelated());
                    this.RaisePropertyChanged(nameof(triggerRelated));
                    break;
            }
        }
        
        public override void onIndexChange(int newValue) {
            if (newValue >= 0 && newValue < UI.patcher.getNarcEntryCount(UI.gameInfo.zoneEntities)) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
                this.RaisePropertyChanged(nameof(selectedIndex));

                currentZoneEntities = new ZoneEntities(UI.patcher.fetchFileFromNarc(UI.gameInfo.zoneEntities, newValue));
                interactables = new ObservableCollection<Interactable>(currentZoneEntities.interactables);
                npcs = new ObservableCollection<NPC>(currentZoneEntities.npcs);
                warps = new ObservableCollection<Warp>(currentZoneEntities.warps);
                triggers = new ObservableCollection<Trigger>(currentZoneEntities.triggers);
                initScripts = new ObservableCollection<InitializationScript>(currentZoneEntities.initializationScripts);
                triggerRelated = new ObservableCollection<TriggerRelated>(currentZoneEntities.triggerRelatedEntries);
                
                this.RaisePropertyChanged(nameof(interactables));
                this.RaisePropertyChanged(nameof(npcs));
                this.RaisePropertyChanged(nameof(warps));
                this.RaisePropertyChanged(nameof(triggers));
                this.RaisePropertyChanged(nameof(initScripts));
                this.RaisePropertyChanged(nameof(triggerRelated));
            }
        }

        public void onRemoveSelectedEntry() {
            if (selectedSubIndices[selectedTab] > -1) {
                switch (selectedTab) {
                    case Interactables:
                        interactables.RemoveAt(selectedSubIndices[selectedTab]);
                        this.RaisePropertyChanged(nameof(interactables));
                        break;
                    case NPCs:
                        npcs.RemoveAt(selectedSubIndices[selectedTab]);
                        this.RaisePropertyChanged(nameof(npcs));
                        break;
                    case Warps:
                        warps.RemoveAt(selectedSubIndices[selectedTab]);
                        this.RaisePropertyChanged(nameof(warps));
                        break;
                    case Triggers:
                        triggers.RemoveAt(selectedSubIndices[selectedTab]);
                        this.RaisePropertyChanged(nameof(triggers));
                        break;
                    case InitializationScripts:
                        initScripts.RemoveAt(selectedSubIndices[selectedTab]);
                        this.RaisePropertyChanged(nameof(initScripts));
                        break;
                    case TriggerRelated:
                        triggerRelated.RemoveAt(selectedSubIndices[selectedTab]);
                        this.RaisePropertyChanged(nameof(triggerRelated));
                        break;
                }
            }
        }

        public override void onSaveChanges() {
            currentZoneEntities.interactables = new List<Interactable>(interactables);
            currentZoneEntities.npcs = new List<NPC>(npcs);
            currentZoneEntities.warps = new List<Warp>(warps);
            currentZoneEntities.triggers = new List<Trigger>(triggers);
            currentZoneEntities.initializationScripts = new List<InitializationScript>(initScripts);
            currentZoneEntities.triggerRelatedEntries = new List<TriggerRelated>(triggerRelated);
        }
    }
}