using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BeaterLibrary.Formats.Text;

namespace SwissArmyKnife.Avalonia.Controls.Pages
{
    public class TextEditor : UserControl
    {
        public TextEditor()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void HandleText(bool Saving)
        {
            void BinaryToText(string path) => this.FindControl<AvaloniaEdit.TextEditor>("TextEditorTextbox").Text =
                new TextContainer().ParseText(Path.GetFullPath(path));

            void TextToBinary(string path) =>
                TextContainer.Serialize(this.FindControl<AvaloniaEdit.TextEditor>("TextEditorTextbox").Text,
                    Path.GetFullPath(path));

            UIUtil.HandleFile(Saving, BinaryToText, TextToBinary, new List<FileDialogFilter>());
        }
    }
}