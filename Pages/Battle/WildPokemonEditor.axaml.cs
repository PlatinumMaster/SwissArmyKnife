using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.ViewModels.Editors;

namespace SwissArmyKnife.Pages.Battle;

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