using System.Threading.Tasks;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace SwissArmyKnife.Avalonia.Handlers {
    internal class MessageHandler {
        public static void ErrorMessage(string title, string caption) {
            MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = $"Error: {title}",
                ContentMessage = caption,
                Icon = Icon.Error,
                Style = Style.None
            }).Show();
        }

        public static Task<ButtonResult> YesNoMessage(string title, string caption) {
            return MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams {
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentTitle = title,
                ContentMessage = caption,
                Icon = Icon.Success,
                Style = Style.Windows
            }).Show();
        }
    }
}