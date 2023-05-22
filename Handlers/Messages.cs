using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Material.Dialog;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using SwissArmyKnife.Avalonia.Handlers;

namespace SwissArmyKnife.Handlers; 

public class Messages {
    private static async Task<DialogResult> DoMessage(IClassicDesktopStyleApplicationLifetime Desktop, string Header, string SupportingText, DialogButton[] Buttons, DialogResult NegativeResult) {
        return await DialogHelper.CreateAlertDialog(
            new AlertDialogBuilderParams {
                ContentHeader = Header,
                SupportingText = SupportingText,
                StartupLocation = WindowStartupLocation.CenterOwner,
                NegativeResult = new DialogResult("No"),
                Borderless = true,
                DialogButtons = Buttons
            }).ShowDialog(Desktop.MainWindow);
    }
    public static Task<DialogResult> YesNoMessage(IClassicDesktopStyleApplicationLifetime Desktop, string Header, string SupportingText) => DoMessage(Desktop, Header, SupportingText, new[] {
        new DialogButton {
            Content = "Yes",
            Result = "Confirm"
        },
        new DialogButton {
            Content = "No",
            Result = "Cancel"
        }
    }, new DialogResult("No"));
}