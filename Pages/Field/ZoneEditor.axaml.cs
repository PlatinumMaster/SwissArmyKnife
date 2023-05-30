using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages.Field; 

public partial class ZoneEditor : UserControl {
    public ZoneEditor() {
        DataContext = new ZoneEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}