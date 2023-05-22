using System.Collections.Generic;

namespace SwissArmyKnife.Models;

public class Preferences {
    public Preferences() {
        ScriptCommandsLink = "https://raw.githubusercontent.com/PlatinumMaster/PokeScriptSDK5/master/yml/";
        BaseROMConfigurationPath = "BaseROM.yml";
    }

    public string ScriptCommandsLink { get; set; }
    public string BaseROMConfigurationPath { get; set; }
    public Dictionary<string, string> ScriptEditorColors { get; set; }
    public Dictionary<string, string> TextEditorColors { get; set; }
}