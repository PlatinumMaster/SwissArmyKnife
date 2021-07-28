using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using BeaterLibrary.Formats.Pokémon;
using SwissArmyKnife.Avalonia.Controls.ViewModels;

namespace SwissArmyKnife.Avalonia.Controls.Pages
{
    public class WildPokemonEditor : ReactiveUserControl<WildPokemonViewModel>
    {
        public WildPokemonEditor()
        {
            DataContext = new WildPokemonViewModel();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void HandleWildPokemonEncounter(bool Saving)
        {
            void BinaryToWildPokemonEncounter(string path) => DataContext = new WildPokemonViewModel(new WildEncounters(new BinaryReader(File.OpenRead(path))));

            void WildPokemonEncounterToBinary(string path) => new WildEncounters()
            {
                GrassEntries = ViewModel.GrassEntries.ToList(),
                GrassDoubleEntries = ViewModel.GrassDoubleEntries.ToList(),
                GrassShakeEntries = ViewModel.GrassShakeEntries.ToList(),
                SurfEntries = ViewModel.SurfEntries.ToList(),
                SurfSpotEntries = ViewModel.SurfSpotEntries.ToList(),
                FishEntries = ViewModel.FishEntries.ToList(),
                FishSpotEntries = ViewModel.FishSpotEntries.ToList(),
            }.Serialize(path);
            UIUtil.HandleFile(Saving, BinaryToWildPokemonEncounter, WildPokemonEncounterToBinary,
                new List<FileDialogFilter>());
        }
    }
}