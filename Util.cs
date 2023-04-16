using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using BeaterLibrary;
using BeaterLibrary.GameInfo;
using Hotswap;
using SwissArmyKnife.Avalonia.Handlers;

namespace SwissArmyKnife.Avalonia.Utils {
    public class UI {
        public static AbstractGameInformation gameInfo;
        public static Patcher patcher { get; set; }

        public static void initializePatcher(string baseROMConfigurationPath, string configurationPath) {
            patcher = new Patcher(baseROMConfigurationPath, configurationPath);
            gameInfo = GameInformation.getGameConfiguration(patcher.getGameCode());
            try {
                if (!CommandUpdateHandler.fetchScriptCommands()) {
                    throw new Exception("Something went wrong when fetching commands!");
                }
            }
            catch (Exception e) {
                // Skip.
            }
        }

        public static void scriptToAssembler(string path, string game, string script, int scriptPlugins) {
            StringBuilder s = new();
            Util.generateCommandAsm(game, "Resources/Scripts", scriptPlugins);
            s.Append($".include \"{game}.s\"{Environment.NewLine}");
            s.Append($"Header:{Environment.NewLine}");
            Regex r = new(@".*Script_\w+:.*");
            foreach (Match m in r.Matches(script))
                s.Append($"\tscript {m.Value.Split(':')[0]}{Environment.NewLine}");
            s.Append($"EndHeader{Environment.NewLine}");

            using (StreamWriter sw = new(path)) {
                var output = string.Join(Environment.NewLine, s.ToString(), script);
                sw.Write(output);
            }
        }

        private static string subprocess(string program, string args) {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo {
                FileName = program,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (!proc.Start()) {
                return
                    $"Error: Program \"{program}\" was not found.\n" +
                    "If this is one of the arm executables and you are not on Windows, make sure you have arm-none-eabi-binutils installed (and in your path), and try again.\n" +
                    "If this is one of the arm executables and you are a Windows user, make sure arm-none-eabi-as and arm-none-eabi-objcopy are in the same folder as SwissArmyKnife, or in a location referred to by your system path.";
            }
            
            StringBuilder Str = new StringBuilder();
            while (!proc.StandardOutput.EndOfStream) {
                Str.Append(proc.StandardOutput.ReadLine());
            }
            
            proc.WaitForExit();

            return Str.ToString();
        }

        public static string assembler(string path, string output) {
            return subprocess("arm-none-eabi-as", $"-mthumb -c \"{path}\" -o \"{output}\"");
        }

        public static string objectCopy(string path, string output) {
            return subprocess("arm-none-eabi-objcopy", $"-O binary \"{path}\" \"{output}\"");
        }

        private static async Task<string> handleFolderFileChoice(bool saving, bool isFile, Window parentInstance,
            List<FileDialogFilter> dialogFilter) {
            string result;
            if (isFile) {
                if (saving) {
                    result = await new SaveFileDialog() { Filters = dialogFilter }.ShowAsync(parentInstance);
                }
                else {
                    string[] resultArray =
                        await new OpenFileDialog {Filters = dialogFilter, AllowMultiple = false}.ShowAsync(
                            parentInstance);
                    result = resultArray != null && resultArray.Length != 0 ? resultArray[0] : null;
                }
            }
            else {
                result = await new OpenFolderDialog().ShowAsync(parentInstance);
            }

            if (result == null || result == string.Empty) throw new OperationCanceledException("Operation canceled!");

            if (result.Length > 0) return result;

            return null;
        }

        public static async Task<string> openFolder(Window parentInstance) {
            return await handleFolderFileChoice(false, false, parentInstance, null);
        }

        public static async Task<string> openFile(Window parentInstance, List<FileDialogFilter> dialogFilter) {
            return await handleFolderFileChoice(false, true, parentInstance, dialogFilter);
        }

        public static async Task<string> saveFile(Window parentInstance, List<FileDialogFilter> dialogFilter) {
            return await handleFolderFileChoice(true, true, parentInstance, dialogFilter);
        }
    }
}