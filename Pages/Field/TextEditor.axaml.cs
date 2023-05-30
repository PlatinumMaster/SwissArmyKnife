using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages.Field;

public partial class TextEditor : UserControl {
    public TextEditor() {
        DataContext = new TextEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
        TextEditorTextbox = this.Get<AvaloniaEdit.TextEditor>(nameof(TextEditorTextbox));
    }

    private void TextEditor_OnKeyDown(object? sender, KeyEventArgs e) {
        if (e.Key == Key.OemPlus && (e.KeyModifiers & KeyModifiers.Control) != 0) {
            TextEditorTextbox.FontSize++;
            TextEditorTextbox.Undo();
        } else if (e.Key == Key.OemMinus && (e.KeyModifiers & KeyModifiers.Control) != 0) {
            TextEditorTextbox.FontSize--;
            TextEditorTextbox.Undo();
        }
        e.Handled = true;
    }
}