namespace SwissArmyKnife.Avalonia.Models {
    public class Preferences {
        public Preferences() {
            ScriptCommandsLink = "https://raw.githubusercontent.com/HelloOO7/PokeScriptSDK5/master/yml/";
            BaseROMConfiguration = "Configuration/BaseROM.yml";
        }

        public string ScriptCommandsLink { get; }
        public string BaseROMConfiguration { get; }
    }
}