using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using BeaterLibrary;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.GameInfo;
using Hotswap;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace SwissArmyKnife.Avalonia
{
    public class UIUtil
    {
        public static AbstractGameInformation CurrentGameInformation;
        public static Patcher BaseROMPatcher { get; set; }
        public static void InitializePatcher(string ConfigurationPath)
        {
            BaseROMPatcher = new Patcher("BaseROM.yml", ConfigurationPath);
            CurrentGameInformation = GameInformation.GetGameConfiguration(BaseROMPatcher.GetGameCode());
        }

        public static void ScriptToAssembler(string path, string game, string script, int[] ScriptPlugins)
        {
            StringBuilder s = new();
            Util.GenerateCommandASM(game, "Resources/Scripts", ScriptPlugins);
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
                CreateNoWindow = true
            };

            proc.Start();
            proc.WaitForExit();

            var ErrorOutput = proc.StandardError.ReadToEnd();

            if (proc.ExitCode != 0)
                throw new Exception(ErrorOutput);
        }

        public static List<string> FetchText(string[] GamePath, int ID, bool UseComments, bool Beautify)
        {
            return TextContainer.ParseText(new MemoryStream(BaseROMPatcher.FetchFileFromNARC(GamePath, ID)),
                    UseComments, Beautify)
                .Split(Environment.NewLine)
                .ToList();
        }

        public static void Assembler(string path, string output)
        {
            Subprocess("arm-none-eabi-as", $"-mthumb -c {path} -o {output}");
        }

        public static void ObjectCopy(string path, string output)
        {
            Subprocess("arm-none-eabi-objcopy", $"-O binary {path} {output}");
        }

        public static async void HandleFile(bool Saving, Action<string> OpenMethod, Action<string> SaveMethod,
            List<FileDialogFilter> Filters)
        {
            try
            {
                // if (Saving)
                // {
                //     string Result = await new SaveFileDialog {Filters = Filters}.ShowAsync(MainWindow.Instance);
                //     if (Result != null)
                //         SaveMethod(Result);
                // }
                // else
                // {
                //     string[] Result = await new OpenFileDialog {Filters = Filters}.ShowAsync(MainWindow.Instance);
                //     if (Result.Length > 0)
                //         OpenMethod(Result.First());
                // }
            }
            catch (Exception e)
            {
                await MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = $"Error while {(Saving ? "saving" : "loading")}",
                        ContentMessage = e.Message,
                        Icon = Icon.Error,
                        Style = Style.None
                    }).Show();
            }
        }
    }
}