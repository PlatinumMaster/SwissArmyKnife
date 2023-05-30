using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages.Field; 

public partial class MapEditor : UserControl {
    public MapEditor() {
        DataContext = new MapEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}