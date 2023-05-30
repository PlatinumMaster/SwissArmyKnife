using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.ViewModels.Editors;

namespace SwissArmyKnife.Pages.Field; 

public partial class ZoneEntitiesEditor : UserControl {
    public ZoneEntitiesEditor() {
        DataContext = new ZoneEntitiesEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}