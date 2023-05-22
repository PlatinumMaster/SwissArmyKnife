using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages {
    public class TextEditor : ReactiveUserControl<TextEditorViewModel> {
        public TextEditor() {
            InitializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new TextEditorViewModel();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}