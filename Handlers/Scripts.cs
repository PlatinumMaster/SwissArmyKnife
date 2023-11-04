using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Avalonia.Markup.Xaml.Templates;
using BeaterLibrary;
using BeaterLibrary.Formats.Scripts;

namespace SwissArmyKnife.Handlers; 

public class Scripts {
    private static List<int> ParsedJumpOffsets = new List<int>();
    public static string PrintMethod(ScriptMethod Method, int ScriptIndex = -1, bool IsAnonymous = false) {
        StringBuilder ConvertedMethod = new StringBuilder();
        ConvertedMethod.AppendLine(IsAnonymous ? $"AnonymousScriptMethod_{Method.Address}:" 
            : $"Script_{ScriptIndex}:");
        List<int> JumpAddresses = Method.Commands.FindAll(Commands => Commands.Type.Equals(Commands.Type == CommandTypes.Jump)).Select(Command => (int)Command.Parameters[^1]).ToList();
        int BaseAddress = Method.Address;
        foreach (Command Cmd in Method.Commands) {
            if (JumpAddresses.Contains(BaseAddress) && !ParsedJumpOffsets.Contains(BaseAddress)) {
                ConvertedMethod.AppendLine($"AnonymousScriptMethod_{BaseAddress}:");
                ParsedJumpOffsets.Append(BaseAddress);
            } else {
                ConvertedMethod.AppendLine($"\t{Cmd}");
            }
            BaseAddress += Cmd.Size();
        }
        return ConvertedMethod.ToString();
    }

    public static void Reset() {
        ParsedJumpOffsets.Clear();
    }
    
    public static void Assemble(string ExportPath, string Script, int Plugin) {
        // Generate include.
        StringBuilder s = new StringBuilder();
        Util.GenerateIncludes(GameWork.Project.GameInfo.Abbreviation, "Resources/Scripts", Plugin);
        
        // Generate headers.
        s.AppendLine($".include \"{GameWork.Project.GameInfo.Abbreviation}.s\"");
        s.AppendLine($"Header:");
        Regex r = new Regex(@".*Script_\w+:.*");
        foreach (Match m in r.Matches(Script)) {
            s.AppendLine($"\tscript {m.Value.Split(':')[0]}");
        }
        s.AppendLine("EndHeader");
        using (StreamWriter sw = new StreamWriter("Temp.s")) {
            sw.Write(string.Join(Environment.NewLine, s.ToString(), Script));
        }
        
        Assembler("Temp.s", "Temp.o");
        ObjectCopy("Temp.o", ExportPath);
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
}