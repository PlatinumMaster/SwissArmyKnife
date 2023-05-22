using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages; 

public class HeaderEditor : ReactiveUserControl<ZoneHeaderViewModel> {
    public HeaderEditor() {
        if (!Design.IsDesignMode)
            DataContext = new ZoneHeaderViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}