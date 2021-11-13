using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using BeaterLibrary.Formats.Maps;
using SwissArmyKnife.Avalonia.ViewModels;

namespace SwissArmyKnife.Avalonia.Pages
{
    public class HeaderEditor : ReactiveUserControl<UserControl>
    {
        public HeaderEditor()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new HeaderViewModel(
                    new MapHeaders(UIUtil.BaseROMPatcher.FetchFileFromNARC(UIUtil.CurrentGameInformation.ZoneHeader, 0))
                        .Headers
                );
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}