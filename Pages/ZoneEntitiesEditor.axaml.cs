using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using BeaterLibrary.Formats.Furniture;
using SwissArmyKnife.Avalonia.Controls.ViewModels;

namespace SwissArmyKnife.Avalonia.Controls.Pages
{
    public class ZoneEntitiesEditor : ReactiveUserControl<ZoneEntitiesViewModel>
    {

        public ZoneEntitiesEditor()
        {
            InitializeComponent();
            DataContext = new ZoneEntitiesViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void HandleZoneEntities(bool Saving)
        {
            void BinaryToZoneEntities(string path) => DataContext = new ZoneEntitiesViewModel(new ZoneEntities(path));

            void ZoneEntitiesToBinary(string path) => new ZoneEntities()
            {
                Interactables = ViewModel.Interactables.ToList(),
                NPCs = ViewModel.NPCs.ToList(),
                Warps = ViewModel.Warps.ToList(),
                Triggers = ViewModel.Triggers.ToList(),
                LevelScripts = ViewModel.InitializationScripts.ToList()
            }.Serialize(path);
            UIUtil.HandleFile(Saving, BinaryToZoneEntities, ZoneEntitiesToBinary, new List<FileDialogFilter>());
        }
    }
}