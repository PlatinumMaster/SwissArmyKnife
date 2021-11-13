using System;
using System.IO;
using System.Xml;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using SwissArmyKnife.Avalonia.Pages;

namespace SwissArmyKnife.Avalonia.Views
{
    public class MainWindow : Window
    {
        private readonly HeaderEditor _HeaderEditor;
        private readonly MapEditor _MapEditor;
        private readonly ZoneEntitiesEditor _OverworldEditor;
        private readonly ScriptEditor _ScriptEditor;
        private readonly TabControl _TabControl;
        private readonly TextEditor _TextEditor;
        private readonly WildPokemonEditor _WildPokemonEditor;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _ScriptEditor = this.FindControl<ScriptEditor>("ScriptEditor");
            if (File.Exists("BeaterSyntax.xshd"))
                _ScriptEditor.FindControl<AvaloniaEdit.TextEditor>("ScriptEditorTextbox").SyntaxHighlighting =
                    HighlightingLoader.Load(new XmlTextReader(File.Open("BeaterSyntax.xshd", FileMode.Open)),
                        HighlightingManager.Instance);
            _TextEditor = this.FindControl<TextEditor>("TextEditor");
            _MapEditor = this.FindControl<MapEditor>("MapEditor");
            _OverworldEditor = this.FindControl<ZoneEntitiesEditor>("OverworldEditor");
            _HeaderEditor = this.FindControl<HeaderEditor>("HeaderEditor");
            _TabControl = this.FindControl<TabControl>("EditorsTabControl");
            _WildPokemonEditor = this.FindControl<WildPokemonEditor>("WildPkmnEditor");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void HandleFileDialogRequest(bool Saving)
        {
            try
            {
                UIUtil.BaseROMPatcher.PatchAndSerialize("TestROM.nds");
            }
            catch (Exception e)
            {
                MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "I/O error",
                        ContentMessage = e.Message,
                        Icon = MessageBox.Avalonia.Enums.Icon.Error,
                        Style = Style.None
                    }).Show();
            }
        }

        private void OnOpenFileClick(object? sender, RoutedEventArgs e)
        {
            HandleFileDialogRequest(false);
        }

        private void OnSaveFileClick(object? sender, RoutedEventArgs e)
        {
            HandleFileDialogRequest(true);
        }
    }
}