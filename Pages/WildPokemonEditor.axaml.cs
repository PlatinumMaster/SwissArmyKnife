using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages;

public class WildPokemonEditor : ReactiveUserControl<WildPokemonViewModel> {
    public WildPokemonEditor() {
        if (!Design.IsDesignMode)
            DataContext = new WildPokemonViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}