using ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Base;

namespace SwissArmyKnife.Avalonia.ViewModels.Main; 

public class FlowViewModel : ViewModelBase {
    private string _CurrentTabName, _WindowTitle;
    public string CurrentTabName {
        get => _CurrentTabName;
        set => this.RaiseAndSetIfChanged(ref _CurrentTabName, value);
    }
    
    public string WindowTitle {
        get => _WindowTitle;
        set => this.RaiseAndSetIfChanged(ref _WindowTitle, value);
    }
}