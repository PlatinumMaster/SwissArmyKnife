using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages.Battle;

public partial class TrainerEditor : UserControl {
    public TrainerEditor() {
        DataContext = new TrainerEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}