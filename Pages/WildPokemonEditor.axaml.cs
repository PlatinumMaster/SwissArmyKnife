using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.GameInfo;
using SwissArmyKnife.Avalonia.Utils;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages {
    public class WildPokemonEditor : ReactiveUserControl<WildPokemonViewModel> {
        public WildPokemonEditor() {
            if (!Design.IsDesignMode)
                DataContext = new WildPokemonViewModel();
            initializeComponent();
        }

        private void initializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) {
            throw new System.NotImplementedException();
        }
    }
}