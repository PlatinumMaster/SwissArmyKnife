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

namespace SwissArmyKnife.Avalonia.Controls
{
    public class GameSelect : Window
    {
        private static string? _Game;
        public string Action = "TerminateWindow";
        private bool[] RadioButtonChecked = new bool[3];

        public GameSelect()
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

        private void HandleRadioButtonCheck(int Index)
        {
            RadioButtonChecked = RadioButtonChecked.Select(x => false).ToArray();
            RadioButtonChecked[Index] = true;
        }

        public new async Task<string> Show(Window parent)
        {
            await ShowDialog(parent);
            if (_Game is null && !Action.Equals("TerminateWindow")) throw new Exception("Game cannot be null.");
            return _Game;
        }

        private void HGSS_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleRadioButtonCheck(0);
        }

        private void BW_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleRadioButtonCheck(1);
        }

        private void B2W2_OnChecked(object? sender, RoutedEventArgs e)
        {
            HandleRadioButtonCheck(2);
        }

        private void OkayButton_OnClick(object? sender, RoutedEventArgs e)
        {
            Action = "Continue";
            switch (RadioButtonChecked.IndexOf(true))
            {
                case 0:
                    _Game = "HGSS";
                    break;
                case 1:
                    _Game = "BW";
                    break;
                case 2:
                    _Game = "B2W2";
                    break;
            }

            Close();
        }

        private void Window_OnClosing(object? sender, CancelEventArgs e)
        {
            if (!(sender as GameSelect).Action.Equals("TerminateWindow"))
                if (!RadioButtonChecked.Any(x => x is true))
                {
                    e.Cancel = true;
                    MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Choose a game!",
                            ContentMessage = "You have to choose a game, man.",
                            Icon = MessageBox.Avalonia.Enums.Icon.Error,
                            Style = Style.None
                        }).Show();
                    Action = "TerminateWindow";
                }
        }
    }
}