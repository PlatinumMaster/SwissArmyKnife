using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using AvaloniaEdit;
using BeaterLibrary;
using BeaterLibrary.Formats.Scripts;
using BeaterLibrary.GameInfo;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.ViewModels
{
    public class ScriptEditorViewModel : ViewModelBase
    {
        private readonly TextEditor Textbox;
        private int _SelectedIndex;

        public ScriptEditorViewModel()
        {
        }

        public ScriptEditorViewModel(TextEditor FindControl) : this()
        {
            SaveFile = ReactiveCommand.Create(() =>
            {
                try
                {
                    UIUtil.BaseROMPatcher.SaveToNARCFolder(UIUtil.CurrentGameInformation.Scripts,
                        SelectedIndex,
                        x =>
                        {
                            UIUtil.BaseROMPatcher.SaveToNARCFolder(UIUtil.CurrentGameInformation.Scripts, SelectedIndex, x =>
                            {
                                UIUtil.ScriptToAssembler(
                                    "Temp.s",
                                    UIUtil.CurrentGameInformation.Title, 
                                    Textbox.Text, 
                                    UIUtil.CurrentGameInformation.GetScriptPluginsByScrID(SelectedIndex)
                                );
                                UIUtil.Assembler("Temp.s", "Temp.o");
                                UIUtil.ObjectCopy("Temp.o", x);
                            });
                        });
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
            Indices = new ObservableCollection<string>(Enumerable.Range(0, UIUtil.BaseROMPatcher.GetNARCEntryCount(UIUtil.CurrentGameInformation.Scripts) - 1).Select(x => x.ToString()));
            Textbox = FindControl;
            SelectedIndex = 0;
        }

        public ObservableCollection<string> Indices { get; set; }

        public ReactiveCommand<Unit, Unit> SaveFile { get; set; }

        public int SelectedIndex
        {
            get => _SelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedIndex, value);
                try
                {
                    Textbox.Text = Util.UnpackScriptContainer(
                        new ScriptContainer(
                            UIUtil.BaseROMPatcher.FetchFileFromNARC(UIUtil.CurrentGameInformation.Scripts, value),
                            Path.Combine("Resources", "Scripts"), UIUtil.CurrentGameInformation.Title,
                            UIUtil.CurrentGameInformation.GetScriptPluginsByScrID(SelectedIndex)
                        ));
                }
                catch (Exception e)
                {
                    Textbox.Text = "Something went wrong when decompiling this script.\n Either there is a syntactically invalid file present, or you attempted to load a level script.";
                }
            }
        }
    }
}