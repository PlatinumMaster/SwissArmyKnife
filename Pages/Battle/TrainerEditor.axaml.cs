using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.ViewModels.Editors;

namespace SwissArmyKnife.Pages.Battle;

public partial class TrainerEditor : UserControl {
    public TrainerEditor() {
        DataContext = new TrainerEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}