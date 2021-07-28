using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using BeaterLibrary.Formats.Furniture;
using BeaterLibrary.Formats.Maps;
using DynamicData.Kernel;
using SwissArmyKnife.Avalonia.Controls.ViewModels;

namespace SwissArmyKnife.Avalonia.Controls.Pages
{
    public class HeaderEditor : ReactiveUserControl<UserControl>
    {
        private MapHeaders Headers;

        public HeaderEditor()
        {
            InitializeComponent();
            this.DataContext = new HeaderViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void HandleMapHeaders(bool Saving)
        {
            void BinaryToMapHeaders(string path)
            {
                Headers = new MapHeaders(path);
                DataContext = new HeaderViewModel(Headers.Headers);
            }

            void MapHeadersToBinary(string path) => Headers.Serialize(path);
            UIUtil.HandleFile(Saving, BinaryToMapHeaders, MapHeadersToBinary, new List<FileDialogFilter>());
        }
    }
}