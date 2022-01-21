using System;
using System.IO;
using System.Xml;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.Pages;
using SwissArmyKnife.Avalonia.Utils;
using SwissArmyKnife.Avalonia.ViewModels;

namespace SwissArmyKnife.Avalonia.Views {
    public class MainWindow : ReactiveWindow<MainWindowViewModel> {
        private readonly ScriptEditor _scriptEditor;
        private readonly TextEditor _textEditor;

        public MainWindow() {
            initializeComponent();
            DataContext = new MainWindowViewModel(this);
#if DEBUG
            this.AttachDevTools();
#endif
            _scriptEditor = this.FindControl<ScriptEditor>("ScriptEditor");
            if (File.Exists("BeaterSyntax.xshd"))
                _scriptEditor.FindControl<AvaloniaEdit.TextEditor>("ScriptEditorTextbox").SyntaxHighlighting =
                    HighlightingLoader.Load(new XmlTextReader(File.Open("BeaterSyntax.xshd", FileMode.Open)),
                        HighlightingManager.Instance);
            _textEditor = this.FindControl<TextEditor>("TextEditorTextbox");
        }

        private void initializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}