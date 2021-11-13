using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SwissArmyKnife.Avalonia.Controls
{
    public class OverlayPluginSelect : Window
    {
        private readonly bool[] _PluginsToUse = new bool[16];
        private int[][] _SelectedPlugins = new int[1][];

        private readonly Dictionary<int, int[]> IndexToPluginIndex = new()
        {
            [0] = new[] {50},
            [1] = new[] {51},
            [2] = new[] {52},
            [3] = new[] {53},
            [4] = new[] {54},
            [5] = new[] {55, 56},
            [6] = new[] {55, 57},
            [7] = new[] {58, 59},
            [8] = new[] {61},
            [9] = new[] {62},
            [10] = new[] {63},
            [11] = new[] {64},
            [12] = new[] {65},
            [13] = new[] {66},
            [14] = new[] {67},
            [15] = new[] {68}
        };

        public OverlayPluginSelect()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async Task<int[][]> Show(Window Parent)
        {
            await ShowDialog(Parent);
            return _SelectedPlugins;
        }

        private void HandleCheckedItem(int Index)
        {
            _PluginsToUse[Index] = !_PluginsToUse[Index];
        }

        private void Overlay50_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(0);
        }

        private void Overlay51_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(1);
        }

        private void Overlay52_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(2);
        }

        private void Overlay53_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(3);
        }

        private void Overlay54_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(4);
        }

        private void Overlays55_56_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(5);
        }

        private void Overlays_55_57_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(6);
        }

        private void Overlays_58_59_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(7);
        }

        private void Overlay61_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(8);
        }

        private void Overlay62_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(9);
        }

        private void Overlay63_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(10);
        }

        private void Overlay64_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(11);
        }

        private void Overlay65_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(12);
        }

        private void Overlay66_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(13);
        }

        private void Overlay67_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(14);
        }

        private void Overlay68_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleCheckedItem(15);
        }

        private void OkayButton_OnClick(object? sender, RoutedEventArgs e)
        {
            List<int[]> _SelectedPlugins = new();
            for (var i = 0; i < _PluginsToUse.Length; ++i)
                if (_PluginsToUse[i])
                    _SelectedPlugins.Add(IndexToPluginIndex[i]);
            this._SelectedPlugins = _SelectedPlugins.ToArray();
            Close();
        }
    }
}