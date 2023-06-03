using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Material.Dialog;
using ReactiveUI;
using SwissArmyKnife.Handlers;

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
    protected int ARC { get; set; }

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

    protected async Task<bool> RefreshPromptConfirm(string Header, string Supporting) {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
            DialogResult Result = await Messages.YesNo(Desktop, Header, Supporting);
            return Result.GetResult.Equals("No");
        }
        return false;
    }
}