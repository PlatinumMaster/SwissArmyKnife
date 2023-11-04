using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Material.Dialog;
using Material.Icons;
using Material.Icons.Avalonia;

namespace SwissArmyKnife.Handlers; 

public class Messages {
    private static async Task<DialogResult> DoMessage(IClassicDesktopStyleApplicationLifetime Desktop, string Header, string SupportingText, DialogButton[] Buttons, MaterialIconExt MsgIcon) {
        Logging.LogStandard($"MessageBox (Icon {nameof(MsgIcon)} spawned, with title \"${Header}\" and content \"{SupportingText}\"");
        return await DialogHelper.CreateAlertDialog(
            new AlertDialogBuilderParams {
                ContentHeader = Header,
                SupportingText = SupportingText,
                StartupLocation = WindowStartupLocation.CenterOwner,
                NegativeResult = new DialogResult("No"),
                Borderless = true,
                DialogButtons = Buttons,
            }).ShowDialog(Desktop.MainWindow);
    }
    public static Task<DialogResult> YesNo(IClassicDesktopStyleApplicationLifetime Desktop, string Header, string SupportingText) => DoMessage(Desktop, Header, SupportingText, new[] {
        new DialogButton {
            Content = "Yes",
            Result = "Confirm"
        },
        new DialogButton {
            Content = "No",
            Result = "Cancel"
        }
    }, new MaterialIconExt(MaterialIconKind.Warning));
    
    public static Task<DialogResult> Generic(IClassicDesktopStyleApplicationLifetime Desktop, string Header, string SupportingText) => DoMessage(Desktop, Header, SupportingText, new[] {
        new DialogButton {
            Content = "Okay",
            Result = "Confirm"
        },
    }, new MaterialIconExt(MaterialIconKind.Information));
    
    public static Task<DialogResult> Error(IClassicDesktopStyleApplicationLifetime Desktop, string Header, string SupportingText) => DoMessage(Desktop, Header, SupportingText, new[] {
        new DialogButton {
            Content = "Okay",
            Result = "Confirm"
        },
    }, new MaterialIconExt(MaterialIconKind.Error));
}