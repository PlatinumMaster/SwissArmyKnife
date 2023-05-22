using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public static AbstractGameInformation GameInfo;
        public static Patcher Patcher { get; set; }

        public static void InitializePatcher(string baseRomConfigurationPath, string configurationPath) {
            Patcher = new Patcher(baseRomConfigurationPath, configurationPath);
            GameInfo = GameInformation.getGameConfiguration(Patcher.getGameCode());
            if (!CommandUpdateHandler.FetchScriptCommands()) {
                throw new Exception("Something is wrong!");
            }
        }

        public static void ScriptToAssembler(string path, string game, string script, int scriptPlugins) {
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

        private static void Subprocess(string program, string args) {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo {
                FileName = program,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            proc.Start();
            proc.WaitForExit();

            var errorOutput = proc.StandardError.ReadToEnd();

            if (proc.ExitCode != 0) {
                throw new Exception(errorOutput);
            }
        }

        public static void Assembler(string path, string output) {
            Subprocess("arm-none-eabi-as", $"-mthumb -c \"{path}\" -o \"{output}\"");
        }

        public static void ObjectCopy(string path, string output) {
            Subprocess("arm-none-eabi-objcopy", $"-O binary \"{path}\" \"{output}\"");
        }

        private static async Task<string> HandleFolderFileChoice(bool saving, bool isFile, Window parentInstance,
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
                    result = resultArray.Length != 0 ? resultArray[0] : null;
                }
            }
            else {
                result = await new OpenFolderDialog().ShowAsync(parentInstance);
            }

            if (result == null || result == string.Empty) throw new OperationCanceledException("Operation canceled!");

            if (result.Length > 0) return result;

            return null;
        }

        public static async Task<string> OpenFolder(Window parentInstance) {
            return await HandleFolderFileChoice(false, false, parentInstance, null);
        }

        public static async Task<string> OpenFile(Window parentInstance, List<FileDialogFilter> dialogFilter) {
            return await HandleFolderFileChoice(false, true, parentInstance, dialogFilter);
        }

        public static async Task<string> SaveFile(Window parentInstance, List<FileDialogFilter> dialogFilter) {
            return await HandleFolderFileChoice(true, true, parentInstance, dialogFilter);
        }
    }
}