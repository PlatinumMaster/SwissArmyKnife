using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls;
using Hotswap.Configuration;
using ReactiveUI;

namespace SwissArmyKnife.ViewModels.Base; 

public abstract class EditorViewModelBase : ViewModelBase {
    public ObservableCollection<TabItem> Tabs { get; set; }
    public ReactiveCommand<Unit, Unit> AddNew { get; }
    public ReactiveCommand<Unit, Unit> RemoveSelected { get; }
    public ReactiveCommand<Unit, Unit> LoadFile { get; }
    public ReactiveCommand<Unit, Unit> SaveChanges { get; }
    public int SelectedIndex { get; set; }
    public int Max { get; set; }
    private int _CurrentTab;
    public int CurrentTab {
        get => _CurrentTab;
        set {
            this.RaiseAndSetIfChanged(ref _CurrentTab, value);
            TryShowTabControl();
        }
    }

    public EditorViewModelBase() {
        SelectedIndex = 0;
        AddNew = ReactiveCommand.Create(OnAddNew);
        RemoveSelected = ReactiveCommand.Create(OnRemoveSelected);
        LoadFile = ReactiveCommand.Create(OnLoadFile);
        SaveChanges = ReactiveCommand.Create(OnSaveChanges);
        Tabs = new ObservableCollection<TabItem>();
    }

    public abstract void OnAddNew();
    public abstract void OnRemoveSelected();
    public abstract void OnLoadFile();
    public abstract void OnSaveChanges();
    protected abstract void TryShowTabControl();
}