using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.ViewModels.Editors;

namespace SwissArmyKnife.Pages.Field;

public partial class ScriptEditor : UserControl {
    public ScriptEditor() {
        DataContext = new ScriptEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
        ScriptEditorTextbox = this.Get<AvaloniaEdit.TextEditor>(nameof(ScriptEditorTextbox));
    }

    private void ScriptEditor_OnKeyDown(object? sender, KeyEventArgs e) {
        if (e.Key == Key.OemPlus && (e.KeyModifiers & KeyModifiers.Control) != 0) {
            ScriptEditorTextbox.FontSize++;
        } else if (e.Key == Key.OemMinus && (e.KeyModifiers & KeyModifiers.Control) != 0) {
            ScriptEditorTextbox.FontSize--;
        }
        e.Handled = true;
    }
}