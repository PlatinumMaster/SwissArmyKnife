﻿using System;
using System.IO;
using System.Reactive;
using AvaloniaEdit.Document;
using BeaterLibrary;
using BeaterLibrary.Formats.Scripts;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public class ScriptEditorViewModel : ViewModelTemplate {
        private int _selectedIndex;
        private TextDocument _textDoc;

        public ScriptEditorViewModel() {
            textDoc = new TextDocument();
            loadScript = ReactiveCommand.Create(() => changeScript(selectedIndex));
            selectedIndex = 0;
            changeScript(selectedIndex);
        }

        public override int selectedIndex {
            get => _selectedIndex;
            set => onIndexChange(value);
        }

        public TextDocument textDoc {
            get => _textDoc;
            set => this.RaiseAndSetIfChanged(ref _textDoc, value);
        }

        public ReactiveCommand<Unit, Unit> loadScript { get; }

        public override void onAddNew() {
            
        }

        private void changeScript(int index) {
            try {
                textDoc.Text = Util.unpackScriptContainer(
                    new ScriptContainer(
                        UI.patcher.fetchFileFromNarc(UI.gameInfo.scripts, index),
                        Path.Combine("Resources", "Scripts"),
                        UI.gameInfo.title,
                        UI.gameInfo.getScriptPluginsByScrId(selectedIndex)
                    ));
            }
            catch (Exception ex) {
                textDoc.Text = "Something went wrong when decompiling this script.\n" + ex;
            }
        }

        public override void onIndexChange(int newValue) {
            if (newValue >= 0 && newValue < UI.patcher.getNarcEntryCount(UI.gameInfo.scripts))
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
        }

        public override void onRemoveSelected(int index) {
            
        }

        public override void onSaveChanges() {
            UI.patcher.saveToNarcFolder(
                UI.gameInfo.scripts,
                _selectedIndex,
                x => {
                    UI.patcher.saveToNarcFolder(UI.gameInfo.scripts, selectedIndex, x => {
                        UI.scriptToAssembler("Temp.s", UI.gameInfo.title, textDoc.Text, UI.gameInfo.getScriptPluginsByScrId(selectedIndex));
                        
                        // Hacky way to detect errors.
                        string ASResult = UI.assembler("Temp.s", "Temp.o");
                        MessageHandler.warnMessage("Assembler output", $"If you see any errors here, there was a compilation error, and the file was not created.\nThe output is as follows:\n\n{ASResult}");
                        if (ASResult.Contains("Error")) {
                            return;
                        }
                        
                        string OBJResult = UI.objectCopy("Temp.o", x);
                        MessageHandler.warnMessage("Objcopy output", $"If you see any errors here, there was a linking error, and the file was not created.\nThe output is as follows:\n\n{OBJResult}");
                        if (OBJResult.Contains("Error")) {
                            return;
                        }

                        File.Delete("Temp.s");
                        File.Delete("Temp.o");
                    });
                });
        }
    }
}