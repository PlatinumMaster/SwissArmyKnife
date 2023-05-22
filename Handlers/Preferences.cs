using System;
using System.Collections.Generic;
using System.IO;
using SwissArmyKnife.Avalonia.Handlers;
using SwissArmyKnife.Models;
using YamlDotNet.Serialization;

namespace SwissArmyKnife.Handlers;

public class Preferences {
    public static Models.Preferences Prefs;
    public static void ReadPreferences(string path) {
        Dictionary<string, string?> preferences = new Dictionary<string, string?> {
            { "ScriptCommandsLink", "https://raw.githubusercontent.com/PlatinumMaster/PokeScriptSDK5/master/yml/" },
            { "BaseROMConfigurationPath", Path.Combine("Configuration", "BaseROM.yml") }
        };

        Dictionary<string, string> scriptEditorColors = new Dictionary<string, string> {
            { "Method", "Cyan" },
            { "Comment", "Green" },
            { "Operator", "Fuchsia" },
            { "Parameter", "Orange" }
        };

        Dictionary<string, string> textEditorColors = new Dictionary<string, string> {
            { "Command", "" },
            { "Comment", "" },
            { "Operator", "" },
            { "Parameter", "" }
        };

        if (!File.Exists(path)) {
            Logging.LogWarning($"Preferences file not found at path \"{path}\"; using default configuration.");
        }
        else {
            Logging.LogStandard($"Loading preferences from {path}...");
            Dictionary<string, Object> deserializedPreferences =
                new Deserializer().Deserialize<Dictionary<string, Object>>(new StringReader(File.ReadAllText(path)));
            if (deserializedPreferences.ContainsKey("ScriptCommandsRepository")) {
                var newScriptRepository = deserializedPreferences["ScriptCommandsRepository"] as string;
                if (newScriptRepository != null) {
                    preferences["ScriptCommandsLink"] = newScriptRepository;
                    Logging.LogStandard($"Changed script repository to {newScriptRepository}.");
                }
                else {
                    Logging.LogWarning($"Invalid script repository link. Using the default.");
                }
            }

            if (deserializedPreferences.ContainsKey("BaseROMConfigurationPath")) {
                var newBaseROMConfigurationPath = deserializedPreferences["BaseROMConfigurationPath"] as string;
                if (newBaseROMConfigurationPath != null) {
                    preferences["BaseROMConfigurationPath"] = newBaseROMConfigurationPath;
                    Logging.LogStandard($"Changed BaseROM configuration path to {newBaseROMConfigurationPath}.");
                }
                else {
                    Logging.LogWarning($"Invalid BaseROM configuration path. Using the default.");
                }
            }

            if (deserializedPreferences.ContainsKey("ScriptEditorColors")) {
                Dictionary<string, string>? newScriptEditorColors = deserializedPreferences["ScriptEditorColors"] as Dictionary<string, string>;
                if (newScriptEditorColors != null) {
                    foreach (KeyValuePair<string, string> kv in newScriptEditorColors)
                        if (scriptEditorColors.ContainsKey(kv.Key)) {
                            scriptEditorColors[kv.Key] = kv.Value;
                            Logging.LogStandard($"Changed the script editor's {kv.Key} color to {kv.Value}.");
                        }
                }
                else {
                    Logging.LogWarning($"Invalid script editor color configuration. Using the default.");
                }
            }

            if (deserializedPreferences.ContainsKey("TextEditorColors")) {
                Dictionary<string, string>? newTextEditorColors =
                    deserializedPreferences["TextEditorColors"] as Dictionary<string, string>;
                if (newTextEditorColors != null) {
                    foreach (KeyValuePair<string, string> kv in newTextEditorColors)
                        if (textEditorColors.ContainsKey(kv.Key)) {
                            textEditorColors[kv.Key] = kv.Value;
                            Logging.LogStandard($"Changed the text editor's {kv.Key} color to {kv.Value}.");
                        }
                }
                else {
                    Logging.LogWarning($"Invalid text editor color configuration. Using the default.");
                }
            }
        }

        Prefs = new Models.Preferences {
            ScriptCommandsLink = preferences["ScriptCommandsLink"],
            BaseROMConfigurationPath = preferences["BaseROMConfigurationPath"],
            ScriptEditorColors = scriptEditorColors,
            TextEditorColors = textEditorColors
        };

        Logging.LogStandard($"Done loading preferences.");
    }

    public static void Serialize(string path) { }
}