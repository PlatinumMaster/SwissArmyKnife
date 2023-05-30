using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.ViewModels.Editors;

namespace SwissArmyKnife.Pages.Field; 

public partial class ZoneEditor : UserControl {
    public ZoneEditor() {
        DataContext = new ZoneEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}