using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using BeaterLibrary.Formats.Scripts;

namespace SwissArmyKnife.Handlers; 

public class ScriptHandler {
    public static string PrintMethod(ScriptMethod Method, bool IsAnonymous = false) {
        StringBuilder ConvertedMethod = new StringBuilder();
        // List<int> JumpAddresses = Method.Commands.FindAll(Commands => Commands.Type.Equals(Commands.Type == CommandTypes.Jump)).Select(Command => (int)Command.Parameters[^1]).ToList();
        // ConvertedMethod.AppendLine(IsAnonymous ? $"AnonymousScriptMethod_{Method.Address}:" : $"Script_{Method.Address}:");
        // for (int Start = Method.Address, CommandPointer = 0; Start < Method.Address + Method.Size; ++CommandPointer) {
        //     if (JumpAddresses.Contains(Start)) {
        //         ConvertedMethod.AppendLine($"AnonymousScriptMethod_{Start}:");
        //     }
        //     ConvertedMethod.AppendLine($"\t{Method.Commands[CommandPointer]}");
        //     Start += Method.Commands[CommandPointer].Size();
        // }
        return ConvertedMethod.ToString();
    }

    public static void CoalesceBranches(List<Link<AnonymousScriptMethod>> Jumps) {
        // List<Link<AnonymousScriptMethod>> Js = new List<Link<AnonymousScriptMethod>>();
        // foreach (Link<AnonymousScriptMethod> Link in Jumps) {
        //     List<Link<AnonymousScriptMethod>> SameEnd = Jumps.FindAll(x =>
        //         x.StartAddress + x.Data.Size == Link.StartAddress + Link.Data.Size);
        //     Link<AnonymousScriptMethod> Max = SameEnd[0];
        //     foreach (Link<AnonymousScriptMethod> Cmp in SameEnd) {
        //         if (Cmp.StartAddress <= Max.StartAddress) {
        //             Max = Cmp;
        //         }
        //     }
        //
        //     if (Js.All(x => x.StartAddress != Max.StartAddress)) {
        //         Js.Add(Max);
        //     }
        // }
        // Js.Clear();
    }
}