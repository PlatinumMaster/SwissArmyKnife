using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace SwissArmyKnife.Avalonia.Controls {
    public class GameSelect : Window {
        private static string? _game;
        private bool[] _radioButtonChecked = new bool[3];
        public string action = "TerminateWindow";

        public GameSelect() {
            initializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void initializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void handleRadioButtonCheck(int index) {
            _radioButtonChecked = _radioButtonChecked.Select(x => false).ToArray();
            _radioButtonChecked[index] = true;
        }

        public async Task<string> show(Window parent) {
            await ShowDialog(parent);
            if (_game is null && !action.Equals("TerminateWindow")) throw new Exception("Game cannot be null.");
            return _game;
        }

        private void HGSS_OnChecked(object? sender, RoutedEventArgs e) {
            handleRadioButtonCheck(0);
        }

        private void BW_OnChecked(object? sender, RoutedEventArgs e) {
            handleRadioButtonCheck(1);
        }

        private void B2W2_OnChecked(object? sender, RoutedEventArgs e) {
            handleRadioButtonCheck(2);
        }

        private void OkayButton_OnClick(object? sender, RoutedEventArgs e) {
            action = "Continue";
            switch (_radioButtonChecked.IndexOf(true)) {
                case 0:
                    _game = "HGSS";
                    break;
                case 1:
                    _game = "BW";
                    break;
                case 2:
                    _game = "B2W2";
                    break;
            }

            Close();
        }

        private void Window_OnClosing(object? sender, CancelEventArgs e) {
            if (!(sender as GameSelect).action.Equals("TerminateWindow"))
                if (!_radioButtonChecked.Any(x => x is true)) {
                    e.Cancel = true;
                    MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Choose a game!",
                            ContentMessage = "You have to choose a game, man.",
                            Icon = MessageBox.Avalonia.Enums.Icon.Error,
                            Style = Style.None
                        }).Show();
                    action = "TerminateWindow";
                }
        }
    }
}