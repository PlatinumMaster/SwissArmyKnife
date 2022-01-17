using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages;

public class MatrixEditor : ReactiveUserControl<MatrixEditorViewModel> {
    public MatrixEditor() {
        if (!Design.IsDesignMode) 
            DataContext = new MatrixEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}