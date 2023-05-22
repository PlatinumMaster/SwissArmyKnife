using System;
using System.IO;
using System.Net;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.Handlers {
    public static class CommandUpdateHandler {
        public static bool FetchScriptCommands() {
            using (var w = new WebClient()) {
                if (!DownloadYml(w, "Base", Path.Combine("Resources", "Scripts", UI.GameInfo.title, "Base.yml")))
                    throw new WebException("Failed to download script commands");
                foreach (var plugin in UI.GameInfo.scriptPlugins)
                    if (!DownloadYml(w, $"Overlay {plugin}",
                            Path.Combine("Resources", "Scripts", UI.GameInfo.title, $"Overlay {plugin}.yml")))
                        throw new WebException($"Failed to download script plugin {plugin}.");

                return true;
            }
        }

        public static bool DownloadYml(WebClient w, string ymlName, string expPath) {
            DirectoryInfo reqPath = Directory.GetParent(expPath);
            if (!reqPath.Exists) {
                reqPath.Create();
            }
            w.DownloadFile(new Uri(GetLinkToYml(ymlName)), expPath);
            return true;
        }

        public static string GetLinkToYml(string ymlName) {
            return $"{PreferencesHandler.Prefs.ScriptCommandsLink}{UI.GameInfo.title}/{ymlName}.yml";
        }
    }
}