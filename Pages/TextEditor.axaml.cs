using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages {
    public class TextEditor : ReactiveUserControl<TextEditorViewModel> {
        public TextEditor() {
            initializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new TextEditorViewModel();
        }

        private void initializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}