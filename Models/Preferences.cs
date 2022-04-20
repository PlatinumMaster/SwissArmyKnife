namespace SwissArmyKnife.Avalonia.Models {
    public class Preferences {
        public Preferences() {
            scriptCommandsLink = "https://raw.githubusercontent.com/PlatinumMaster/PokeScriptSDK5/master/yml/";
            baseROMConfigurationPath = "Configuration/BaseROM.yml";
        }

        public string scriptCommandsLink { get; }
        public string baseROMConfigurationPath { get; }
    }
}