using System;
using System.IO;
using System.Net;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.Handlers {
    public static class CommandUpdateHandler {
        public static bool fetchScriptCommands() {
            using (var w = new WebClient()) {
                if (!downloadYML(w, "Base", Path.Combine("Resources", "Scripts", UI.gameInfo.title, "Base.yml")))
                    throw new WebException("Failed to download script commands");
                foreach (var plugin in UI.gameInfo.scriptPlugins)
                    if (!downloadYML(w, $"Overlay {plugin}",
                            Path.Combine("Resources", "Scripts", UI.gameInfo.title, $"Overlay {plugin}.yml")))
                        throw new WebException($"Failed to download script plugin {plugin}.");

                return true;
            }
        }

        public static bool downloadYML(WebClient w, string YMLName, string expPath) {
            try {
                if (!Directory.Exists(Directory.GetParent(expPath).FullName))
                    Directory.CreateDirectory(Directory.GetParent(expPath).FullName);
                w.DownloadFile(new Uri(getLinkToYML(YMLName)), expPath);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public static string getLinkToYML(string YMLName) {
            return $"{PreferencesHandler.prefs.scriptCommandsLink}{YMLName}.yml";
        }
    }
}