using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using BeaterLibrary.GameInfo;
using SwissArmyKnife.Avalonia.ViewModels;

namespace SwissArmyKnife.Avalonia.Pages
{
    public class ZoneEntitiesEditor : ReactiveUserControl<ZoneEntitiesViewModel>
    {
        public ZoneEntitiesEditor()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new ZoneEntitiesViewModel(
                    UIUtil.BaseROMPatcher.GetNARCEntryCount(UIUtil.CurrentGameInformation.ZoneEntities));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}