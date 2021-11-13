using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels;

namespace SwissArmyKnife.Avalonia.Pages
{
    public class TextEditor : ReactiveUserControl<TextEditorViewModel>
    {
        public TextEditor()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new TextEditorViewModel(
                    this.FindControl<AvaloniaEdit.TextEditor>("TextEditorTextbox"),
                    UIUtil.BaseROMPatcher.GetNARCEntryCount(UIUtil.CurrentGameInformation.SystemsText),
                    UIUtil.BaseROMPatcher.GetNARCEntryCount(UIUtil.CurrentGameInformation.MapText)
                );
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}