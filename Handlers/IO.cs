using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace SwissArmyKnife.Handlers; 

public class IO {
    private static async Task<string?> HandleFolderFileChoice(bool saving, bool isFile, Window parentInstance, List<FileDialogFilter> dialogFilter) {
        string? result;
        if (isFile) {
            if (saving) {
                result = await new SaveFileDialog {
                    Filters = dialogFilter
                }.ShowAsync(parentInstance);
            }
            else {
                string[]? resultArray = await new OpenFileDialog {
                    Filters = dialogFilter
                }.ShowAsync(parentInstance);
                result = resultArray != null && resultArray.Length != 0 ? resultArray[0] : null;
            }
        } else {
            result = await new OpenFolderDialog().ShowAsync(parentInstance);
        }

        return string.IsNullOrEmpty(result) ? String.Empty : result;
    }

    public static async Task<string?> OpenFolder(Window parentInstance) => await HandleFolderFileChoice(false, false, parentInstance, null);
    public static async Task<string?> OpenFile(Window parentInstance, List<FileDialogFilter> dialogFilter) => await HandleFolderFileChoice(false, true, parentInstance, dialogFilter);
    public static async Task<string?> SaveFile(Window parentInstance, List<FileDialogFilter> dialogFilter) => await HandleFolderFileChoice(true, true, parentInstance, dialogFilter);
    
    public static async Task<byte[]> TryReadBinaryFile(Window Instance, List<FileDialogFilter> Filters) {
        string? FilePath = await OpenFile(Instance, Filters);
        if (!FilePath.Equals("")) {
            return await File.ReadAllBytesAsync(FilePath);
        }
        return null;
    }

    public static async Task TryWriteBinaryFile(Window Instance, byte[] Buffer, List<FileDialogFilter> Filters) {
        string? FilePath = await SaveFile(Instance, Filters);
        if (!FilePath.Equals("")) {
            await File.WriteAllBytesAsync(FilePath, Buffer);
        }
    }
}