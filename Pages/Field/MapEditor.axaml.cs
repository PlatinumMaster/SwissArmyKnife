using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.ViewModels.Editors;

namespace SwissArmyKnife.Pages.Field; 

public partial class MapEditor : UserControl {
    public MapEditor() {
        DataContext = new MapEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}