using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SwissArmyKnife.Handlers {
    public class Commands {
        public static async Task<bool> Fetch() {
            string ScriptResBase = Path.Combine("Resources", "Scripts", GameWork.Project.GameInfo.Codename);
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

        public static string GetLinkToYML(string ymlName) => $"{Preferences.Prefs.ScriptCommandsLink}{GameWork.Project.GameInfo.Codename}/{ymlName}";
    }
}