using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using SwissArmyKnife.Avalonia.Controls.Views;

namespace SwissArmyKnife.Avalonia
{
    public class UIUtil
    {
        public static void ScriptToAssembler(string path, string game, string script, int[][] ScriptPlugins)
        {
            StringBuilder s = new();
            BeaterLibrary.Util.GenerateCommandASM(game, "Resources/Scripts", ScriptPlugins);
            s.Append($".include \"{game}.s\"{Environment.NewLine}");
            s.Append($"Header:{Environment.NewLine}");
            Regex r = new(@".*Script_\w+:.*");
            foreach (Match m in r.Matches(script))
                s.Append($"\tscript {m.Value.Split(':')[0]}{Environment.NewLine}");
            s.Append($"EndHeader{Environment.NewLine}");

            using (StreamWriter sw = new(path))
            {
                string output = string.Join(Environment.NewLine, s.ToString(), script);
                sw.Write(output);
            }
        }

        private static void Subprocess(string Program, string Args)
        {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo
            {
                FileName = Program,
                Arguments = Args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            proc?.Start();
            proc.WaitForExit();

            string? ErrorOutput = proc.StandardError.ReadToEnd();

            if (proc.ExitCode != 0)
                throw new Exception(ErrorOutput);
        }

        public static void Assembler(string path, string output) =>
            Subprocess("arm-none-eabi-as.exe", $"-mthumb -c {path} -o {output}");
        public static void ObjectCopy(string path, string output) =>
            Subprocess("arm-none-eabi-objcopy.exe", $"-O binary {path} {output}");

        public static async void HandleFile(bool Saving, Action<string> OpenMethod, Action<string> SaveMethod,
            List<FileDialogFilter> Filters)
        {
            try
            {
                if (Saving)
                {
                    string Result = await new SaveFileDialog {Filters = Filters}.ShowAsync(MainWindow.Instance);
                    if (Result != null)
                        SaveMethod(Result);
                }
                else
                {
                    string[] Result = await new OpenFileDialog {Filters = Filters}.ShowAsync(MainWindow.Instance);
                    if (Result.Length > 0)
                        OpenMethod(Result.First());
                }
            }
            catch (Exception e)
            {
                MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = $"Error while {(Saving ? "saving" : "loading")}",
                        ContentMessage = e.Message,
                        Icon = MessageBox.Avalonia.Enums.Icon.Error,
                        Style = Style.None
                    }).Show();
            }
        }
        
    }
}