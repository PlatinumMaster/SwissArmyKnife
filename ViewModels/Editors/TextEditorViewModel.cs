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

        public bool UseMapText {
            get => _useMapText;
            set => OnUseMapTextChange(value);
        }

        public override int SelectedIndex {
            get => _selectedIndex;
            set => OnIndexChange(value);
        }

        public TextDocument TextDoc {
            get => _textDoc;
            set => this.RaiseAndSetIfChanged(ref _textDoc, value);
        }
        
        public ReactiveCommand<Unit, Unit> LoadText { get; }
        public TextEditorViewModel() {
            TextDoc = new TextDocument();
            LoadText = ReactiveCommand.Create(() => ChangeText(SelectedIndex));
            UseMapText = true;
        }

        public override void OnAddNew() {
            
        }

        private void ChangeText(int newValue) {
            _currentContainer = new TextContainer(UI.Patcher.fetchFileFromNarc(UseMapText ? UI.GameInfo.mapText : UI.GameInfo.systemsText, newValue));
            TextDoc.Text = _currentContainer.fetchTextAsString(true, true);
        }
        
        public override void OnIndexChange(int newValue) {
            if (newValue >= 0 && newValue <
                UI.Patcher.getNarcEntryCount(UseMapText ? UI.GameInfo.mapText : UI.GameInfo.systemsText)) {
                this.RaiseAndSetIfChanged(ref _selectedIndex, newValue);
            }
        }

        public override void OnRemoveSelected(int index) {
            
        }

        public override void OnSaveChanges() {
            UI.Patcher.saveToNarcFolder(UseMapText ? UI.GameInfo.mapText : UI.GameInfo.systemsText, SelectedIndex,
                x => _currentContainer.serialize(TextDoc.Text, x));
        }

        private void OnUseMapTextChange(bool newValue) {
            this.RaiseAndSetIfChanged(ref _useMapText, newValue);
            SelectedIndex = 0;
        }
    }
}