using System;
using System.IO;
using System.Reactive;
using AvaloniaEdit.Document;
using BeaterLibrary.Formats.Text;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public class TextEditorViewModel : ViewModelTemplate {
        private int _selectedIndex;
        private TextDocument _textDoc;
        private bool _useMapText;
        private TextContainer _currentContainer;

        public bool useMapText {
            get => _useMapText;
            set => onUseMapTextChange(value);
        }

        public override int selectedIndex {
            get => _selectedIndex;
            set => onIndexChange(value);
        }

        public TextDocument textDoc {
            get => _textDoc;
            set => this.RaiseAndSetIfChanged(ref _textDoc, value);
        }
        
        public ReactiveCommand<Unit, Unit> loadText { get; }
        public TextEditorViewModel() {
            textDoc = new TextDocument();
            loadText = ReactiveCommand.Create(() => changeText(selectedIndex));
            useMapText = true;
        }

        public override void onAddNew() {
            
        }

        private void changeText(int newValue) {
            _currentContainer = new TextContainer(UI.patcher.fetchFileFromNarc(useMapText ? UI.gameInfo.mapText : UI.gameInfo.systemsText, newValue));
            textDoc.Text = _currentContainer.fetchTextAsString(true, true);
        }
        
        public override void onIndexChange(int newValue) {
            if (newValue >= 0 && newValue <
                UI.patcher.getNarcEntryCount(useMapText ? UI.gameInfo.mapText : UI.gameInfo.systemsText)) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
            }
        }

        public override void onRemoveSelected(int index) {
            
        }

        public override void onSaveChanges() {
            UI.patcher.saveToNarcFolder(useMapText ? UI.gameInfo.mapText : UI.gameInfo.systemsText, selectedIndex,
                x => _currentContainer.serialize(textDoc.Text, x));
        }

        private void onUseMapTextChange(bool newValue) {
            this.RaiseAndSetIfChanged(ref _useMapText, newValue);
            selectedIndex = 0;
        }
    }
}