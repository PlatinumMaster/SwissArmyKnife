using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages; 

public class HeaderEditor : ReactiveUserControl<HeaderViewModel> {
    public HeaderEditor() {
        if (!Design.IsDesignMode)
            DataContext = new HeaderViewModel();
        initializeComponent();
    }

    private void initializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}