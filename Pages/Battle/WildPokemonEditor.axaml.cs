using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages.Battle;

public partial class WildEncounterEditor : UserControl {
    public WildEncounterEditor() {
        if (!Design.IsDesignMode)
            DataContext = new WildEncounterEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}