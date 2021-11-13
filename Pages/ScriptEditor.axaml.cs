using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels;

namespace SwissArmyKnife.Avalonia.Pages
{
    public class ScriptEditor : ReactiveUserControl<ScriptEditorViewModel>
    {
        public ScriptEditor()
        {
            InitializeComponent();
            DataContext = new ScriptEditorViewModel(this.FindControl<AvaloniaEdit.TextEditor>("ScriptEditorTextbox"));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // public void HandleScript(bool Saving)
        // {
        //     async void BinaryToScript(string path)
        //     {
        //         GameSelect GS = new();
        //         string? Game = null;
        //         while (Game is null)
        //         {
        //             Game = await GS.Show(MainWindow.Instance);
        //             if (GS.Action.Equals("TerminateWindow"))
        //                 return;
        //         }
        // 
        //         ScriptContainer p;
        //         try
        //         {
        //              p = new(path, Path.Combine("Resources", "Scripts"), Game,
        //                 await new OverlayPluginSelect().Show(MainWindow.Instance));
        //         }
        //         catch (Exception e)
        //         {
        //             await MessageBox.Avalonia.MessageBoxManager
        //                 .GetMessageBoxStandardWindow(new MessageBoxStandardParams
        //                 {
        //                     ButtonDefinitions = ButtonEnum.Ok,
        //                     ContentTitle = "Script decompilation error",
        //                     ContentMessage = e.Message,
        //                     Icon = Icon.Error,
        //                     Style = Style.None
        //                 }).Show();
        //             return;
        //         }
        //         this.FindControl<AvaloniaEdit.TextEditor>("ScriptEditorTextbox").Text = Util.UnpackScriptContainer(p);
        //     }
        // 
        //     async void ScriptToBinary(string path)
        //     {
        //         UIUtil.ScriptToAssembler("Temp.s", await new GameSelect().Show(MainWindow.Instance),
        //             this.FindControl<AvaloniaEdit.TextEditor>("ScriptEditorTextbox").Text,
        //             await new OverlayPluginSelect().Show(MainWindow.Instance));
        //         Task.Run(() => UIUtil.Assembler("Temp.s", "Temp.o")).Wait();
        //         Task.Run(() => UIUtil.ObjectCopy("Temp.o", $"\"{path}\"")).Wait();
        //     }
        // 
        //     UIUtil.HandleFile(Saving, BinaryToScript, ScriptToBinary, new List<FileDialogFilter>());
        // }
    }
}