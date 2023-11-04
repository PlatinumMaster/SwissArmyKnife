using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.ViewModels.Editors;

namespace SwissArmyKnife.Pages.Field;

public partial class TextEditor : UserControl {
    public TextEditor() {
        DataContext = new TextEditorViewModel();
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
        // TextEditorTextbox = this.Get<AvaloniaEdit.TextEditor>(nameof(TextEditorTextbox));
    }

    private void TextEditor_OnKeyDown(object? sender, KeyEventArgs e) {
        // if (e.Key == Key.OemPlus && (e.KeyModifiers & KeyModifiers.Control) != 0) {
        //     TextEditorTextbox.FontSize++;
        // } else if (e.Key == Key.OemMinus && (e.KeyModifiers & KeyModifiers.Control) != 0) {
        //     TextEditorTextbox.FontSize--;
        // }
        e.Handled = true;
    }
}