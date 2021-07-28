using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FileDialogFilter = Avalonia.Controls.FileDialogFilter;
using BeaterLibrary;
using BeaterLibrary.Formats.Furniture;
using BeaterLibrary.Formats.Scripts;
using BeaterLibrary.Formats.Text;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using SwissArmyKnife.Avalonia.Controls.Pages;
using SwissArmyKnife.Avalonia.Controls.ViewModels;

namespace SwissArmyKnife.Avalonia.Controls.Views
{
    public partial class MainWindow : Window
    {
        private readonly ScriptEditor _ScriptEditor;
        private readonly TextEditor _TextEditor;
        private readonly MapEditor _MapEditor;
        private readonly ZoneEntitiesEditor _OverworldEditor;
        private readonly TabControl _TabControl;
        private readonly HeaderEditor _HeaderEditor;
        private readonly WildPokemonEditor _WildPokemonEditor;
        public static Window Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            _ScriptEditor = this.FindControl<ScriptEditor>("ScriptEditor");
            _ScriptEditor.FindControl<AvaloniaEdit.TextEditor>("ScriptEditorTextbox").SyntaxHighlighting =
                HighlightingLoader.Load(new XmlTextReader(File.Open("BeaterSyntax.xshd", FileMode.Open)),
                    HighlightingManager.Instance);
            _TextEditor = this.FindControl<TextEditor>("TextEditor");
            _MapEditor = this.FindControl<MapEditor>("MapEditor");
            _OverworldEditor = this.FindControl<ZoneEntitiesEditor>("OverworldEditor");
            _HeaderEditor = this.FindControl<HeaderEditor>("HeaderEditor");
            _TabControl = this.FindControl<TabControl>("EditorsTabControl");
            _WildPokemonEditor = this.FindControl<WildPokemonEditor>("WildPkmnEditor");
            Instance = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void HandleFileDialogRequest(bool Saving)
        {
            try
            {
                switch (_TabControl.SelectedIndex)
                {
                    case 0:
                        _ScriptEditor.HandleScript(Saving);
                        break;
                    case 1:
                        _TextEditor.HandleText(Saving);
                        break;
                    case 2:
                        _MapEditor.HandleContainer(Saving);
                        break;
                    case 3:
                        _OverworldEditor.HandleZoneEntities(Saving);
                        break;
                    case 4:
                        _HeaderEditor.HandleMapHeaders(Saving);
                        break;
                    case 5:
                        _WildPokemonEditor.HandleWildPokemonEncounter(Saving);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "I/O error",
                        ContentMessage = e.Message,
                        Icon = MessageBox.Avalonia.Enums.Icon.Error,
                        Style = Style.None
                    }).Show();
            }
        }

        private void OnOpenFileClick(object? sender, RoutedEventArgs e) => HandleFileDialogRequest(false);
        private void OnSaveFileClick(object? sender, RoutedEventArgs e) => HandleFileDialogRequest(true);
    }
}