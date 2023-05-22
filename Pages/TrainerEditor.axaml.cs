using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages;

public class TrainerEditor : ReactiveUserControl<TrainerEditorViewModel> {
    public TrainerEditor() {
        InitializeComponent();
        if (!Design.IsDesignMode)
            DataContext = new TrainerEditorViewModel();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}