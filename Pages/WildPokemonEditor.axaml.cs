using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages;

public class WildPokemonEditor : ReactiveUserControl<WildPokemonViewModel> {
    public WildPokemonEditor() {
        if (!Design.IsDesignMode)
            DataContext = new WildPokemonViewModel();
        initializeComponent();
    }

    private void initializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}