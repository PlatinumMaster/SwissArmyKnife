﻿using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages.Field;

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
            ScriptEditorTextbox.Undo();
        }
        else if (e.Key == Key.OemMinus && (e.KeyModifiers & KeyModifiers.Control) != 0) {
            ScriptEditorTextbox.FontSize--;
            ScriptEditorTextbox.Undo();
        }

        e.Handled = true;
    }
}