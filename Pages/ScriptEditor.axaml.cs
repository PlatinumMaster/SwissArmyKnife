using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages {
    public class ScriptEditor : ReactiveUserControl<ScriptEditorViewModel> {
        public ScriptEditor() {
            initializeComponent();
            if (!Design.IsDesignMode) 
                DataContext = new ScriptEditorViewModel();
        }

        private void initializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}