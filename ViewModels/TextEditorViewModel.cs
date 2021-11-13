using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using AvaloniaEdit;
using BeaterLibrary.Formats.Text;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.ViewModels
{
    public class TextEditorViewModel : ViewModelBase
    {
        private readonly int GameTextCount;
        private readonly int SystemsTextCount;
        private readonly TextEditor Textbox;
        private int _SelectedIndex;
        private bool _UseMapText;

        public TextEditorViewModel()
        {
        }

        public TextEditorViewModel(TextEditor FindControl, int SystemsTextCount, int GameTextCount) : this()
        {
            SaveFile = ReactiveCommand.Create(() =>
            {
                try
                {
                    UIUtil.BaseROMPatcher.SaveToNARCFolder(UseMapText
                            ? UIUtil.CurrentGameInformation.MapText
                            : UIUtil.CurrentGameInformation.SystemsText,
                        SelectedIndex,
                        x => TextContainer.Serialize(FindControl.Text, x)
                    );
                }
                catch (Exception e)
                {
                    MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "I/O error",
                            ContentMessage = e.Message,
                            Icon = Icon.Error,
                            Style = Style.None
                        }).Show();
                }
            });
            this.SystemsTextCount = SystemsTextCount;
            this.GameTextCount = GameTextCount;
            UseMapText = false;
            Textbox = FindControl;
            SelectedIndex = 0;
        }

        public ObservableCollection<string> Indices { get; set; }
        public ReactiveCommand<Unit, Unit> SaveFile { get; }

        public bool UseMapText
        {
            get => _UseMapText;
            set
            {
                this.RaiseAndSetIfChanged(ref _UseMapText, value);
                Indices = new ObservableCollection<string>(Enumerable
                    .Range(0, (value ? GameTextCount : SystemsTextCount) - 1).Select(x => x.ToString()));
                this.RaisePropertyChanged("Indices");
            }
        }

        public int SelectedIndex
        {
            get => _SelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedIndex, value);
                Textbox.Text =
                    TextContainer.ParseText(
                        new MemoryStream(
                            UIUtil.BaseROMPatcher.FetchFileFromNARC(UseMapText
                                    ? UIUtil.CurrentGameInformation.MapText
                                    : UIUtil.CurrentGameInformation.SystemsText,
                                value)),
                        true,
                        true);
            }
        }
    }
}