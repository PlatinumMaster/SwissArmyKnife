using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace SwissArmyKnife.Avalonia.Handlers;

public class Preferences {
    public static Models.Preferences Prefs;
    public static void ReadPreferences(string path) {
        Dictionary<string, string?> Preferences = new Dictionary<string, string?> {
            { "ScriptCommandsLink", "https://raw.githubusercontent.com/HelloOO7/PokeScriptSDK5/master/yml/" },
            { "BaseROMConfigurationPath", Path.Combine("Configuration", "BaseROM.yml") }
        };

        Dictionary<string, string> ScriptEditorColors = new Dictionary<string, string> {
            { "Method", "Cyan" },
            { "Comment", "Green" },
            { "Operator", "Fuchsia" },
            { "Parameter", "Orange" }
        };

        Dictionary<string, string> TextEditorColors = new Dictionary<string, string> {
            { "Command", "" },
            { "Comment", "" },
            { "Operator", "" },
            { "Parameter", "" }
        };

        if (!File.Exists(path)) {
            Logging.LogWarning($"Preferences file not found at path \"{path}\"; using default configuration.");
        } else {
            Logging.LogStandard($"Loading preferences from {path}...");
            Dictionary<string, Object> DeserializedPreferences =
                new Deserializer().Deserialize<Dictionary<string, Object>>(new StringReader(File.ReadAllText(path)));
            if (DeserializedPreferences.ContainsKey("ScriptCommandsRepository")) {
                var NewScriptRepository = DeserializedPreferences["ScriptCommandsRepository"] as string;
                if (NewScriptRepository != null) {
                    Preferences["ScriptCommandsLink"] = NewScriptRepository;
                    Logging.LogStandard($"Changed script repository to {NewScriptRepository}.");
                }
                else {
                    Logging.LogWarning($"Invalid script repository link. Using the default.");
                }
            }

            if (DeserializedPreferences.ContainsKey("BaseROMConfigurationPath")) {
                var NewBaseRomConfigurationPath = DeserializedPreferences["BaseROMConfigurationPath"] as string;
                if (NewBaseRomConfigurationPath != null) {
                    Preferences["BaseROMConfigurationPath"] = NewBaseRomConfigurationPath;
                    Logging.LogStandard($"Changed BaseROM configuration path to {NewBaseRomConfigurationPath}.");
                }
                else {
                    Logging.LogWarning($"Invalid BaseROM configuration path. Using the default.");
                }
            }

            if (DeserializedPreferences.ContainsKey("ScriptEditorColors")) {
                Dictionary<string, string>? NewScriptEditorColors = DeserializedPreferences["ScriptEditorColors"] as Dictionary<string, string>;
                if (NewScriptEditorColors != null) {
                    foreach (KeyValuePair<string, string> KVP in NewScriptEditorColors)
                        if (ScriptEditorColors.ContainsKey(KVP.Key)) {
                            ScriptEditorColors[KVP.Key] = KVP.Value;
                            Logging.LogStandard($"Changed the script editor's {KVP.Key} color to {KVP.Value}.");
                        }
                }
                else {
                    Logging.LogWarning($"Invalid script editor color configuration. Using the default.");
                }
            }

            if (DeserializedPreferences.ContainsKey("TextEditorColors")) {
                Dictionary<string, string>? NewTextEditorColors =
                    DeserializedPreferences["TextEditorColors"] as Dictionary<string, string>;
                if (NewTextEditorColors != null) {
                    foreach (KeyValuePair<string, string> KVP in NewTextEditorColors)
                        if (TextEditorColors.ContainsKey(KVP.Key)) {
                            TextEditorColors[KVP.Key] = KVP.Value;
                            Logging.LogStandard($"Changed the text editor's {KVP.Key} color to {KVP.Value}.");
                        }
                }
                else {
                    Logging.LogWarning($"Invalid text editor color configuration. Using the default.");
                }
            }
        }

        Prefs = new Models.Preferences {
            ScriptCommandsLink = Preferences["ScriptCommandsLink"],
            BaseROMConfigurationPath = Preferences["BaseROMConfigurationPath"],
            ScriptEditorColors = ScriptEditorColors,
            TextEditorColors = TextEditorColors
        };

        Logging.LogStandard($"Done loading preferences.");
    }

    public static void Serialize(string path) { }
}