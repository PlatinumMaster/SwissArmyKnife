using System;
using System.IO;
using System.Reactive;
using AvaloniaEdit.Document;
using BeaterLibrary;
using BeaterLibrary.Formats.Scripts;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public class ScriptEditorViewModel : ViewModelTemplate {
        private int _selectedIndex;
        private TextDocument _textDoc;

        public ScriptEditorViewModel() {
            TextDoc = new TextDocument();
            LoadScript = ReactiveCommand.Create(() => ChangeScript(SelectedIndex));
            SelectedIndex = 0;
            ChangeScript(SelectedIndex);
        }

        public override int SelectedIndex {
            get => _selectedIndex;
            set => OnIndexChange(value);
        }

        public TextDocument TextDoc {
            get => _textDoc;
            set => this.RaiseAndSetIfChanged(ref _textDoc, value);
        }

        public ReactiveCommand<Unit, Unit> LoadScript { get; }

        public override void OnAddNew() {
            
        }

        private void ChangeScript(int index) {
            try {
                TextDoc.Text = Util.unpackScriptContainer(
                    new ScriptContainer(
                        UI.Patcher.fetchFileFromNarc(UI.GameInfo.scripts, index),
                        Path.Combine("Resources", "Scripts"),
                        UI.GameInfo.title,
                        UI.GameInfo.getScriptPluginsByScrId(SelectedIndex)
                    ));
            }
            catch (Exception ex) {
                TextDoc.Text = "Something went wrong when decompiling this script.\n" + ex;
            }
        }

        public override void OnIndexChange(int newValue) {
            if (newValue >= 0 && newValue < UI.Patcher.getNarcEntryCount(UI.GameInfo.scripts))
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
        }

        public override void OnRemoveSelected(int index) {
            
        }

        public override void OnSaveChanges() {
            UI.Patcher.saveToNarcFolder(
                UI.GameInfo.scripts,
                _selectedIndex,
                x => {
                    UI.Patcher.saveToNarcFolder(UI.GameInfo.scripts, SelectedIndex, x => {
                        UI.ScriptToAssembler("Temp.s", UI.GameInfo.title, TextDoc.Text,
                            UI.GameInfo.getScriptPluginsByScrId(SelectedIndex));
                        UI.Assembler("Temp.s", "Temp.o");
                        UI.ObjectCopy("Temp.o", x);
                        File.Delete("Temp.s");
                        File.Delete("Temp.o");
                    });
                });
        }
    }
}