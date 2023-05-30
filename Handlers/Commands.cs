using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SwissArmyKnife.Avalonia.Handlers {
    public class Commands {
        public static async Task<bool> Fetch() {
            string ScriptResBase = Path.Combine("Resources", "Scripts", GetScriptGame(GameWork.Project.GameInfo.ROMCode));
            string NetWkPath = Path.Combine("Resources", "NetWk");

            DirectoryInfo BaseDir = new DirectoryInfo(ScriptResBase), NetWkDir = new DirectoryInfo(NetWkPath);
            if (!BaseDir.Exists) {
                BaseDir.Create();
            }
            
            if (!NetWkDir.Exists) {
                NetWkDir.Create();
            }

            if (!await Net.DownloadFile(GetLinkToYML("Base.yml"), Path.Combine(NetWkPath, "Base.yml"))) {
                NetWkDir.Delete(true);
                return false;
            }
            
            foreach (KeyValuePair<int, int[]> PluginMap in GameWork.Project.GameInfo.ScriptPlugins) {
                string OVLYML = $"Overlay {PluginMap.Key}.yml";
                if (!await Net.DownloadFile(GetLinkToYML(OVLYML), Path.Combine(NetWkPath, OVLYML))) {
                    NetWkDir.Delete(true);
                    return false;
                }
            }

            foreach (FileInfo File in NetWkDir.GetFiles()) {
                File.CopyTo(Path.Combine(ScriptResBase, $"{File.Name}"), true);
            }
            
            NetWkDir.Delete(true);
            return true;
        }
        
        private static string GetScriptGame(string ROMCode) {
            switch (ROMCode.Substring(0, 3).ToUpper()) {
                case "IRA":
                case "IRB":
                    return "BW";
                case "IRD":
                case "IRE":
                    return "B2W2";
                default:
                    return "UNKNOWN";
            }
        }

        public static string GetLinkToYML(string ymlName) => $"{Preferences.Prefs.ScriptCommandsLink}{GetScriptGame(GameWork.Project.GameInfo.ROMCode)}/{ymlName}";
    }
}