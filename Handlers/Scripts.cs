using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}